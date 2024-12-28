# OSK.MessageBus
These sets of projects aim to help facilitate sending messages, or transmissions, across a variety of message buses. By using an abstraction layer,
consuming projects do not need to know the entire process that is required to fully setup and handle communication to and from a bus. Instead, by using
a common abstraction, messages can be broadcasted across the entire set of implemented message buses as needed.


# OSK.MessageBus.Abstractions
Provides a set of abstraction/top level classes that can be used for dependency reference without adding a dependency on the core implementation. Consumers
wanting to send messages to their message buses will need to utilize the `IMessageEventBroadcaster`. This interface is the only necessary object for sending
messages, assuming one or more message buses have been added to the dependency container. When sending a transmission, users can target a specific message bus 
for their message to be sent to or can send to all of the integrated buses in the dependency container. By default, the transmissions are sent to all available
message buses.

# OSK.MessageBus.Events.Abstractions
Provides a set of abstraction/top level classes for event management. The core class used by the projects is the `IMessageEvent`, which provides a set of common
properties all message transmissions will have available. A couple of concrete helper abstraction classes are also available to make implementing messages a 
little easier.

# OSK.MessageBus.Application
Applications that need a message bus running in the background, so that processes can run asynchronously, will need a way to manage the receivers. The Application
project provides a simple hosted service that will be added to the dependency container and can run in the background. For custom management, users can either manage
their receivers directly by requesting the list of `IMessageReceiverGroupBuilder` which will allow creating the receivers as needed or by using the 
`IMessageEventReceiverManager` to start or stop their receivers as needed

# Usage: Message Bus Integrations
To use different message bus integrations, consumers will want to implement or add an implementation that utilizes the library in the dependency container. To do this,
consumers will need to ensure the core services are added via one of the `AddMessageBus` extensions. Transmissions are sent via transmitters to their listening receivers,
and how the transmissions are handled can be unique per transmission channel or shared across global configuration middlewares. To configure globally shared setups, users 
can use the `AddMessageBus` overload that provides a configuration action for `MessageBusConfigurationOptions`, which will be used when building the receivers. 

Once core services have been added, users can add a message bus to their dependency container via the `AddMessageEventTransmitter` extension. This extension provides a way 
to add message transmitters, or message buses, along with their unique receivers that will listen for the transmissions that are sent. This is a strongly typed extension to 
help with understanding what message bus is being added to the dependency container at a glance. When adding a transmitter/receiver configuration, a transmitter id must be 
provided so that transmission broadcasts will have a way to target specific message buses. These transmitter ids must be unique. `IMessageEventTransmitter` implementations will provide the
core implementation for how to communicate with the actual message bus, such as RabbitMQ, Kaefka, Local, or other message buses and should be created with any implementation.

Along with a core transmitter integration, integrations will need to implement an associated `IMessageEventReceiver`. These will be what listen for transmissions that are sent
via a transmitter. These will be started and stopped either by the provided `IMessageEventReceiverManager` or some other management mechanism to start and stop each receiver. 
Receivers should be disposed of when stopped and the `IMessageReceiverGroupBuilder` is used to create new receivers whenever the manager is started. Implementations of `IMessageEventReceiver`
should expect several arguments to be provided by the builder in their constructors: a unique string `receiverId`, a `MessageEventTransmissionDelegate`, and an `IServiceProvider`.
A base implementation that can help with integrations is provided with `MessageEventReceiverBase`. The passed in delegate should be utilized when a transmission is received from a transmitter 
and allows middlewares to interact with the transmission until a final handler is used to fully process the message for an application. Middlewares and handlers can be easily added
using the `MessageEventReceiverBuilderExtensions`. When using `AddMessageEventTransmitter`, an `IMessageReceiverGroupBuilder` object will be provided via a lambda that will give access
to the consumer to set shared transmission channel middleware and grant access to an `IMessageReceiverBuilder` that is used to set the custom handlers and middlewares specific to a receiver.

Construction of the transmitter and receiver objects occurs as needed via internal `MessageEventTransmitterDescriptor` and `MessageEventReceiverDescriptor` objects that are created for integration 
via the library extensions when adding new message buses.

While specific implementation configuration for a specific message bus transmitter can vary, below is a generic example of how to create an implementation for a transmitter/receiver integration:

1. Add a custom message bus transmitter
```
public class CustomMessageBusTransmitter: IMessageEventTransmitter 
{
   public Task TransmitAsync(message, transmissionOptions, cancellationToken) 
   {
     // Custom message bus logic to transmit the message to the server/local machine/etc.
   }
}
```

2. Add a message bus receiver for the transmitter
```
public class CustomMessageBusReceiver(receiverId, delegate, serviceProvider, customArgument1, customerArgument2, ...)
    : MessageEventReceiver(receiverId, delegate, serviceProvider)
{
    public void Dispose() 
    {
       // Dispose of specific receiver resources    
    }

    public void Start() 
    {
      // Specific receiver logic to listen for message bus transmitter transmissions
    }
}
```

3. Add the transmitter/receiver pair to the dependency container
```
public static ServiceCollectionExtensions 
{
    public static IServiceCollection AddCustomMessageBus(services) 
    {
        // Ensure a message bus has been added
        services.AddMessageBus(busOptions => {
            busOptions.AddGlobalReceiverConfiguration(receiverBuilder => {  
                // Add globally shared middleware used by all receivers across all message bus transmitters
            })
        })

        services.AddTransmitter<CustomMessageBusTransmitter, CustomMessageBusReceiver>("NamedCustomMessageBusId", groupBuilder => {
           groupBuilder.AddConfigurator(receiverBuilder => {
            // Add shared middleware across all receivers for this transmitter
           })

           groupBuilder.AddMessageEventReceiver(receiverId, customParameters, receiverBuilder => {
            // Add custom middleware specific to this receiver
           })

           // For instances where an integration may want to implement more than one receiver type, receivers that
           // inherit from the receiver type in the AddTransmitter call can be added as well
           groupBuilder.AddMessageEventReceiver<CustomReceiverSubClass>(...);
        });
    }
}
```


# Usage: Consumer Broadcasts
After the core library services have been added via any of the `AddMessageBus` extensions and at least one integration for a message bus has been added, consumers 
can send messages to their message buses via the `IMessageEventBroadcaster` interface. This provides a generic method that allows sending any message type, and a 
set of broadcast options that can allow setting delayed messages and targetting specific message bus transmitters for transmissions by using their named transmitterId
that was provided when adding the transmitter to the dependency container. Broadcasts will attempt to be sent to the target message buses and a `BroadcastResult` will
be returned to provide information to the success of the messages being sent to all the message bus transmitters. For consumers, one of three scenarios can occur and 
information relating to the result can be determined checking the status code of the output of the broadcaster function call. For quick reference, the 3 scenarios and 
related responses are as follows:
* Consumer sends a broadcast that succeeds across all targeted message buses. The response should be `HttpStatusCode.OK`
* Consumer sends a broadcast that succeeds across some of the targeted message buses, but fails on others. The response should be `HttpStatusCode.MultiStatus`
* Consumer sends a broadcast that fails across all target message buses. The response should be `HttpStatusCode.InternalServerError`

For all of the above scenarios, the broadcast result will provide exception information for any of the failed transmitters along with their transmitter ids to allow for
any retry handling that might be necessary by the consumer.

An example consumer usage might be:
```
 public class Service(IMessageEventBroadcaster broadcaster, ...) {
    ...

    public Task DoSomethingAsync() {
       ...

       var broadCastResult = await braodcaster.BroadcastMessageAsync(new CustomMessage(), configuration => {
        configuration.TargetTransmitterIds = [ "CustomBusA", "CustomBusZ" ]
       });


       if (broadCastResult.Code.StatusCode == HttpStatusCode.OK) 
       {
         return;
       }

       var failedTransmissions = broadCastResult.TransmissionResults.Where(transmission => !transmission.Successful);
       
       // Failed transmission logic
       ...
    }
 } 
```

Extensions for the message broadcaster are available that should hopefully make it easier to use in some cases. These can be found in `MessageEventBroadcasterExtensions`