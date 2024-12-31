using Microsoft.Extensions.DependencyInjection;
using OSK.Transmissions.Abstractions;
using OSK.Transmissions.Messages.Abstractions;
using OSK.Transmissions.Models;
using OSK.Transmissions.Ports;
using System;
using System.Threading.Tasks;

namespace OSK.Transmissions
{
    public static class MessageReceiverBuilderExtensions
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

        public static IMessageReceiverBuilder UseHandler<TMessage, THandler>(this IMessageReceiverBuilder builder,
            THandler handler)
            where TMessage : IMessage
            where THandler : IMessageTransmissionHandler<TMessage>
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return builder.UseHandler<TMessage>(handler.HandleMessageAsync);
        }

        public static IMessageReceiverBuilder UseHandler<TMessage>(this IMessageReceiverBuilder builder,
            Func<IMessageTransmissionContext<TMessage>, Task> handleFunc)
            where TMessage : IMessage
        {
            if (handleFunc == null)
            {
                throw new ArgumentNullException(nameof(handleFunc));
            }

            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TMessage> typedContext)
                {
                    return handleFunc(typedContext);
                }

                return next(context);
            });
        }

        public static IMessageReceiverBuilder UseHandler<TMessage, THandler>(this IMessageReceiverBuilder builder)
            where TMessage : IMessage
            where THandler : IMessageTransmissionHandler<TMessage>
        {
            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TMessage> typedContext)
                {
                    var handler = context.Services.GetRequiredService<THandler>();
                    return handler.HandleMessageAsync(typedContext);
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

        public static IMessageReceiverBuilder UseMiddleware<TMessage>(this IMessageReceiverBuilder builder,
            IMessageTransmissionMiddleware<TMessage> middleware)
            where TMessage : IMessage
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            return builder.UseMiddleware<TMessage>(middleware.InvokeAsync);
        }

        public static IMessageReceiverBuilder UseMiddleware<TMessage>(this IMessageReceiverBuilder builder,
            Func<IMessageTransmissionContext<TMessage>, MessageTransmissionDelegate, Task> middlewareFunc)
            where TMessage : IMessage
        {
            if (middlewareFunc == null)
            {
                throw new ArgumentNullException(nameof(middlewareFunc));
            }

            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TMessage> typedContext)
                {
                    return middlewareFunc(typedContext, next);
                }

                return next(context);
            });
        }

        public static IMessageReceiverBuilder UseMiddleware<TMessage, TMiddleware>(this IMessageReceiverBuilder builder)
            where TMessage : IMessage
            where TMiddleware : IMessageTransmissionMiddleware<TMessage>
        {
            return builder.Use(next => context =>
            {
                if (context is IMessageTransmissionContext<TMessage> typedContext)
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
