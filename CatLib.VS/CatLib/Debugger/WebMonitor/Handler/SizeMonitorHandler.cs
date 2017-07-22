﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections.Generic;

namespace CatLib.Debugger.WebMonitor.Handler
{
    /// <summary>
    /// 基于尺寸大小的监控处理器(字节)
    /// </summary>
    public sealed class SizeMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; private set; }

        /// <summary>
        /// 监控的名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 单位映射
        /// </summary>
        private readonly Dictionary<long, string> unitMapping;

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit { get; private set; }

        /// <summary>
        /// 实时的监控值
        /// </summary>
        public string Value
        {
            get
            {
                double value = 0;
                var longValue = (long)callback.Invoke();
                foreach (var unit in unitMapping)
                {
                    if (longValue >= unit.Key)
                    {
                        continue;
                    }
                    value = longValue / (unit.Key / (double)1024);
                    Unit = unit.Value;
                    break;
                }
                return value.ToString("#0.00");
            }
        }

        /// <summary>
        /// 回调获取结果
        /// </summary>
        private readonly Func<object> callback;

        /// <summary>
        /// 尺寸监控处理器
        /// </summary>
        /// <param name="name">监控名字</param>
        /// <param name="tags">标签</param>
        /// <param name="callback">回调获取结果</param>
        public SizeMonitorHandler(string name , string[] tags , Func<object> callback)
        {
            Guard.NotEmptyOrNull(name, "name");
            Guard.Requires<ArgumentNullException>(tags != null);
            Guard.Requires<ArgumentNullException>(callback != null);
            Name = name;
            Tags = tags;
            this.callback = callback;
            unitMapping = new Dictionary<long, string>
            {
                { 1024 , "unit.size.b"},
                { 1048576 , "unit.size.kb" },
                { 1073741824 ,"unit.size.mb" },
                { 1099511627776 , "unit.size.gb" },
                { 1125899906842624 , "unit.size.tb" },
                { long.MaxValue , "unit.size.pb" }
            };
        }
    }
}
