﻿
using System;
namespace FXSharp.EA.NewsBox
{
    // should be careful about margin level and margin call
    public class MoneyManagement
    {
        private double riskExposure;
        private double balance;

        public MoneyManagement(double riskExposure, double balance)
        {
            this.riskExposure = riskExposure;
            this.balance = balance;
        }

        private double RiskValue { get { return (riskExposure * balance) / 100; } }

        private double ValuePerPips { get { return 10; } } // should check to pips calculator 

        public double CalculateLotSize(MagicBoxOrder magicBoxOrder)
        {
            return Math.Round(RiskValue / (ValuePerPips * Pips(magicBoxOrder.StopLoss)), 2);
        }

        private double Pips(double point)
        {
            return point / 10;
        }
    }
}
