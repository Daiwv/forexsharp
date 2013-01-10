﻿using FXSharp.TradingPlatform.Exts;

namespace FXSharp.EA.NewsBox
{
    public class OrderAlreadyRunning : IOrderState
    {
        private OrderWatcher orderManager;
        private Order order;
        private AutoCloseOrder autoClose;

        public OrderAlreadyRunning(OrderWatcher orderManager, Order order)
        {
            this.orderManager = orderManager;
            this.order = order;
            this.autoClose = new AutoCloseOrder();
        }

        public void Manage()
        {
            // should trail and lock profit

            // should create timer for expired

            if (autoClose.IsExpired)
            {
                order.Close();
                orderManager.MagicBoxCompleted();
            }
            else if (!order.IsOpen)
            {
                orderManager.MagicBoxCompleted();
            }
        }

    }
}
