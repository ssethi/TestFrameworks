﻿namespace Caliburn.PresentationFramework.Screens
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used to gather the results from multiple child elements which may or may not prevent closing.
    /// </summary>
    /// <typeparam name="T">The type of child element.</typeparam>
    public class DefaultCloseStrategy<T> : ICloseStrategy<T>
    {
        List<T> closable;
        IEnumerator<T> enumerator;
        bool finalResult;
        Action<bool, IEnumerable<T>> callback;
        readonly bool closeConductedItemsWhenConductorCannotClose;

        /// <summary>
        /// Creates an instance of the class.
        /// </summary>
        /// <param name="closeConductedItemsWhenConductorCannotClose">Indicates that even if all conducted items are not closable, those that are should be closed.</param>
        public DefaultCloseStrategy(bool closeConductedItemsWhenConductorCannotClose) {
            this.closeConductedItemsWhenConductorCannotClose = closeConductedItemsWhenConductorCannotClose;
        }

        /// <summary>
        /// Executes the strategy.
        /// </summary>
        /// <param name="toClose">Items that are requesting close.</param>
        /// <param name="callback">The action to call when all enumeration is complete and the close results are aggregated.
        /// The bool indicates whether close can occur. The enumerable indicates which children should close if the parent cannot.</param>
        public void Execute(IEnumerable<T> toClose, Action<bool, IEnumerable<T>> callback)
        {
            enumerator = toClose.GetEnumerator();
            this.callback = callback;
            closable = new List<T>();
            finalResult = true;

            Evaluate(true);
        }

        void Evaluate(bool result)
        {
            finalResult = finalResult && result;

            if(!enumerator.MoveNext())
            {
                callback(finalResult, closeConductedItemsWhenConductorCannotClose ? closable : new List<T>());
                closable = null;
            }
            else
            {
                var current = enumerator.Current;
                var guard = current as IGuardClose;
                if(guard != null)
                {
                    guard.CanClose(canClose =>{
                        if(canClose)
                            closable.Add(current);

                        Evaluate(canClose);
                    });
                }
                else
                {
                    closable.Add(current);
                    Evaluate(true);
                }
            }
        }
    }
}