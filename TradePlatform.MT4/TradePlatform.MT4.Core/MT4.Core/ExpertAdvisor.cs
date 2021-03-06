using System;
using System.Collections.Generic;

namespace TradePlatform.MT4.Core
{
	public abstract class ExpertAdvisor : MqlHandler
	{
		protected ExpertAdvisor()
		{
		}

		protected abstract int DeInit();

		protected abstract int Init();

		protected internal override string ResolveMethod(string methodName, List<string> parameters)
		{
			string str = methodName;
			string str1 = str;
			if (str != null)
			{
				if (str1 == "Init")
				{
					int num = this.Init();
					return num.ToString();
				}
				else
				{
					if (str1 == "Start")
					{
						int num1 = this.Start();
						return num1.ToString();
					}
					else
					{
						if (str1 == "DeInit")
						{
							int num2 = this.DeInit();
							return num2.ToString();
						}
					}
				}
			}
			throw new Exception("No method found");
		}

		protected abstract int Start();
	}
}