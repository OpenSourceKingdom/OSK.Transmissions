using Microsoft.Extensions.DependencyInjection;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using OSK.MessageBus.Models;
using OSK.MessageBus.Ports;
using System;
using System.Threading.Tasks;

namespace OSK.MessageBus
{
    public static class MessageEventReceiverBuilderExtensions
    {
        #region Handlers

        public static IMessageReceiverBuilder UseHandler(this IMessageReceiverBuilder builder,
            IMessageTransmissionHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return builder.UseHandler(handler.HandleTransmissionAsync);
        }

        public static IMessageReceiverBuilder UseHandler(this IMessageReceiverBuilder builder,
            Func<IMessageTransmissionContext, Task> handlerFunc)
        {
            if (handlerFunc == null)
            {
                throw new ArgumentNullException(nameof(handlerFunc));
            }

            builder.Use(_ => context => handlerFunc(context));
            return builder;
        }

        public static IMessageReceiverBuilder UseHandler<THandler>(this IMessageReceiverBuilder builder)
            where THandler : IMessageTransmissionHandler
        {
            return builder.Use(_ => context =>
            {
                var handler = context.Services.GetRequiredService<THandler>();
                return handler.HandleTransmissionAsync(context);
            });
        }

        public static IMessageReceiverBuilder UseHandler<TEvent, THandler>(this IMessageReceiverBuilder builder,
            THandler handler)
            where TEvent : IMessage
            where THandler : IMessageTransmissionHandler<TEvent>
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return builder.UseHandler<TEvent>(handler.HandleEventAsync);
        }

        public static IMessageReceiverBuilder UseHandler<TEvent>(this IMessageReceiverBuilder builder,
            Func<IMessageTransmissionContext<TEvent>, Task> handleFunc)
            where TEvent : IMessage
        {
            if (handleFunc == null)
            {
                throw new ArgumentNullException(nameof(handleFunc));
            }

            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TEvent> typedContext)
                {
                    return handleFunc(typedContext);
                }

                return next(context);
            });
        }

        public static IMessageReceiverBuilder UseHandler<TEvent, THandler>(this IMessageReceiverBuilder builder)
            where TEvent : IMessage
            where THandler : IMessageTransmissionHandler<TEvent>
        {
            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TEvent> typedContext)
                {
                    var handler = context.Services.GetRequiredService<THandler>();
                    return handler.HandleEventAsync(typedContext);
                }

                return next(context);
            });
        }

        #endregion

        #region Middleware

        public static IMessageReceiverBuilder UseMiddleware(this IMessageReceiverBuilder builder,
            IMessageTransmissionMiddleware middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            return builder.UseMiddleware(middleware.InvokeAsync);
        }

        public static IMessageReceiverBuilder UseMiddleware(this IMessageReceiverBuilder builder,
            Func<IMessageTransmissionContext, MessageTransmissionDelegate, Task> middlewareFunc)
        {
            if (middlewareFunc == null)
            {
                throw new ArgumentNullException(nameof(middlewareFunc));
            }

            return builder.Use(next => context => middlewareFunc(context, next));
        }

        public static IMessageReceiverBuilder UseMiddleware<TMiddleware>(this IMessageReceiverBuilder builder)
            where TMiddleware : IMessageTransmissionMiddleware
        {
            return builder.Use(next => context =>
            {
                var middleware = context.Services.GetRequiredService<TMiddleware>();
                return middleware.InvokeAsync(context, next);
            });
        }

        public static IMessageReceiverBuilder UseMiddleware<TEvent>(this IMessageReceiverBuilder builder,
            IMessageTransmissionMiddleware<TEvent> middleware)
            where TEvent : IMessage
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            return builder.UseMiddleware<TEvent>(middleware.InvokeAsync);
        }

        public static IMessageReceiverBuilder UseMiddleware<TEvent>(this IMessageReceiverBuilder builder,
            Func<IMessageTransmissionContext<TEvent>, MessageTransmissionDelegate, Task> middlewareFunc)
            where TEvent : IMessage
        {
            if (middlewareFunc == null)
            {
                throw new ArgumentNullException(nameof(middlewareFunc));
            }

            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TEvent> typedContext)
                {
                    return middlewareFunc(typedContext, next);
                }

                return next(context);
            });
        }

        public static IMessageReceiverBuilder UseMiddleware<TEvent, TMiddleware>(this IMessageReceiverBuilder builder)
            where TEvent : IMessage
            where TMiddleware : IMessageTransmissionMiddleware<TEvent>
        {
            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TEvent> typedContext)
                {
                    var middleware = context.Services.GetRequiredService<TMiddleware>();
                    return middleware.InvokeAsync(typedContext, next);
                }

                return next(context);
            });
        }

        #endregion
    }
}
