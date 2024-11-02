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

        public static IMessageEventReceiverBuilder UseHandler(this IMessageEventReceiverBuilder builder,
            IMessageEventHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return builder.UseHandler(handler.HandleEventAsync);
        }

        public static IMessageEventReceiverBuilder UseHandler(this IMessageEventReceiverBuilder builder,
            Func<IMessageEventContext, Task> handlerFunc)
        {
            if (handlerFunc == null)
            {
                throw new ArgumentNullException(nameof(handlerFunc));
            }

            builder.Use(_ => context => handlerFunc(context));
            return builder;
        }

        public static IMessageEventReceiverBuilder UseHandler<THandler>(this IMessageEventReceiverBuilder builder)
            where THandler : IMessageEventHandler
        {
            return builder.Use(_ => context =>
            {
                var handler = context.Services.GetRequiredService<THandler>();
                return handler.HandleEventAsync(context);
            });
        }

        public static IMessageEventReceiverBuilder UseHandler<TEvent, THandler>(this IMessageEventReceiverBuilder builder,
            THandler handler)
            where TEvent : IMessageEvent
            where THandler : IMessageEventHandler<TEvent>
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return builder.UseHandler<TEvent>(handler.HandleEventAsync);
        }

        public static IMessageEventReceiverBuilder UseHandler<TEvent>(this IMessageEventReceiverBuilder builder,
            Func<IMessageEventContext<TEvent>, Task> handleFunc)
            where TEvent : IMessageEvent
        {
            if (handleFunc == null)
            {
                throw new ArgumentNullException(nameof(handleFunc));
            }

            return builder.Use(next => context =>
            {
                if (context is IMessageEventContext<TEvent> typedContext)
                {
                    return handleFunc(typedContext);
                }

                return next(context);
            });
        }

        public static IMessageEventReceiverBuilder UseHandler<TEvent, THandler>(this IMessageEventReceiverBuilder builder)
            where TEvent : IMessageEvent
            where THandler : IMessageEventHandler<TEvent>
        {
            return builder.Use(next => context =>
            {
                if (context is IMessageEventContext<TEvent> typedContext)
                {
                    var handler = context.Services.GetRequiredService<THandler>();
                    return handler.HandleEventAsync(typedContext);
                }

                return next(context);
            });
        }

        #endregion

        #region Middleware

        public static IMessageEventReceiverBuilder UseMiddleware(this IMessageEventReceiverBuilder builder,
            IMessageEventMiddleware middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            return builder.UseMiddleware(middleware.InvokeAsync);
        }

        public static IMessageEventReceiverBuilder UseMiddleware(this IMessageEventReceiverBuilder builder,
            Func<IMessageEventContext, MessageEventDelegate, Task> middlewareFunc)
        {
            if (middlewareFunc == null)
            {
                throw new ArgumentNullException(nameof(middlewareFunc));
            }

            return builder.Use(next => context => middlewareFunc(context, next));
        }

        public static IMessageEventReceiverBuilder UseMiddleware<TMiddleware>(this IMessageEventReceiverBuilder builder)
            where TMiddleware : IMessageEventMiddleware
        {
            return builder.Use(next => context =>
            {
                var middleware = context.Services.GetRequiredService<TMiddleware>();
                return middleware.InvokeAsync(context, next);
            });
        }

        public static IMessageEventReceiverBuilder UseMiddleware<TEvent>(this IMessageEventReceiverBuilder builder,
            IMessageEventMiddleware<TEvent> middleware)
            where TEvent : IMessageEvent
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            return builder.UseMiddleware<TEvent>(middleware.InvokeAsync);
        }

        public static IMessageEventReceiverBuilder UseMiddleware<TEvent>(this IMessageEventReceiverBuilder builder,
            Func<IMessageEventContext<TEvent>, MessageEventDelegate, Task> middlewareFunc)
            where TEvent : IMessageEvent
        {
            if (middlewareFunc == null)
            {
                throw new ArgumentNullException(nameof(middlewareFunc));
            }

            return builder.Use(next => context =>
            {
                if (context is IMessageEventContext<TEvent> typedContext)
                {
                    return middlewareFunc(typedContext, next);
                }

                return next(context);
            });
        }

        public static IMessageEventReceiverBuilder UseMiddleware<TEvent, TMiddleware>(this IMessageEventReceiverBuilder builder)
            where TEvent : IMessageEvent
            where TMiddleware : IMessageEventMiddleware<TEvent>
        {
            return builder.Use(next => context =>
            {
                if (context is IMessageEventContext<TEvent> typedContext)
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
