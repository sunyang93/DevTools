using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Data
{
    /// <summary>
    /// 优惠券生成规则
    /// </summary>
    public class CourtesyCardRule
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 券的类型
        /// </summary>
        public CourtesyCardType CourtesyCardType { get; private set; }

        /// <summary>
        /// 适用的对象
        /// </summary>
        public TargetObject TargetObject { get; private set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public TermOfValidity TermOfValidity { get; private set; }

        /// <summary>
        /// 适用的时段
        /// </summary>
        public TimeInterval TimeInterval { get; private set; }

        /// <summary>
        /// 规则说明
        /// </summary>
        public string RuleDescription { get; private set; }

        public string RuleExpression { get; private set; }

        public string EvalExpression { get; private set; }

        public CourtesyCardRule(string name,
                                CourtesyCardType courtesyCardType,
                                TargetObject targetObject,
                                TermOfValidity termOfValidity,
                                TimeInterval timeInterval,
                                string ruleDescription,
                                string ruleExpression,
                                string evalExpression)
        {
            Name = name;
            CourtesyCardType = courtesyCardType;
            TargetObject = targetObject;
            TermOfValidity = termOfValidity;
            TimeInterval = timeInterval;
            RuleDescription = ruleDescription;
            RuleExpression = ruleExpression;
            EvalExpression = evalExpression;
            switch (TargetObject)
            {
                case TargetObject.Product:
                    Description = "产品";
                    break;
                case TargetObject.Store:
                    Description = "店铺";
                    break;
                case TargetObject.Order:
                    Description = "订单";
                    break;
            }
            switch (CourtesyCardType)
            {
                case CourtesyCardType.FullCouponReduction:
                    Description += "-满减券";
                    break;
                case CourtesyCardType.DiscountCoupon:
                    Description += "折扣券";
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 券的类型
    /// </summary>
    public enum CourtesyCardType
    {
        /// <summary>
        /// 满减券
        /// </summary>
        FullCouponReduction=1,
        /// <summary>
        /// 折扣券
        /// </summary>
        DiscountCoupon
    }

    /// <summary>
    /// 适用的对象
    /// </summary>
    public enum TargetObject
    {
        /// <summary>
        /// 产品
        /// </summary>
        Product=1,
        /// <summary>
        /// 店铺
        /// </summary>
        Store,
        /// <summary>
        /// 订单
        /// </summary>
        Order
    }

    /// <summary>
    /// 有效期
    /// </summary>
    public class TermOfValidity
    {
        public bool NoLimit { get; private set; }

        public int? Limit { get; private set; }

        public LimitType? LimitType { get; private set; }

        public string Description { get; private set; }

        public TermOfValidity(bool noLimit = true, int? limit = null, LimitType? limitType = null)
        {
            NoLimit = noLimit;
            Limit = limit;
            LimitType = limitType;
            if (NoLimit)
            {
                Description = "永久有效";
            }
            else
            {
                ArgumentNullException.ThrowIfNull(limit, nameof(limit));
                ArgumentNullException.ThrowIfNull(limitType, nameof(limitType));
                switch (LimitType)
                {
                    case Data.LimitType.Hour:
                        Description = $"{limit}小时内有效";
                        break;
                    case Data.LimitType.Day:
                        Description = $"{limit}天内有效";
                        break;
                    case Data.LimitType.Month:
                        Description = $"{limit}个月内有效";
                        break;
                    case Data.LimitType.Year:
                        Description = $"{limit}年内有效";
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public enum LimitType
    {
        /// <summary>
        /// 小时
        /// </summary>
        Hour=1,
        /// <summary>
        /// 天
        /// </summary>
        Day,
        /// <summary>
        /// 月份
        /// </summary>
        Month,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }

    /// <summary>
    /// 适用的时段
    /// </summary>
    public class TimeInterval
    {
        /// <summary>
        /// 全时段可用
        /// </summary>
        public bool NoLimit { get; private set; }

        /// <summary>
        /// 开始时间(格式 00:00:00)
        /// </summary>
        public string? Begin { get; private set; }

        /// <summary>
        /// 截止时间(格式 00:00:00)
        /// </summary>
        public string? End { get; private set; }

        public TimeInterval(bool noLimit = true, string? begin = null, string? end = null)
        {
            NoLimit = noLimit;
            if (noLimit == false)
            {
                ArgumentException.ThrowIfNullOrEmpty(begin, nameof(begin));
                ArgumentException.ThrowIfNullOrEmpty(end, nameof(end));
                var beginFragments = begin.Trim().Split(':');
                if (beginFragments.Length != 3)
                {
                    throw new ArgumentException("Invalid format", nameof(begin));
                }
                for(var i=0;i<beginFragments.Length;i++)
                {
                    if (int.TryParse(beginFragments[i], out var value))
                    {
                        if (i == 0)
                        {
                            if (value < 0 || value > 24)
                            {
                                throw new ArgumentOutOfRangeException(nameof(begin), "小时只能为00-24");
                            }
                            else
                            {
                                if (value < 0 || value > 60)
                                {
                                    throw new ArgumentOutOfRangeException(nameof(begin), "分钟和秒只能为00-60");
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("无效的值", nameof(begin));
                    }
                }
                var endFragments = begin.Trim().Split(':');
                if (endFragments.Length != 3)
                {
                    throw new ArgumentException("无效的格式", nameof(begin));
                }
                for (var i = 0; i < endFragments.Length; i++)
                {
                    if (int.TryParse(endFragments[i], out var value))
                    {
                        if (i == 0)
                        {
                            if (value < 0 || value > 24)
                            {
                                throw new ArgumentOutOfRangeException(nameof(end), "小时只能为00-24");
                            }
                            else
                            {
                                if (value < 0 || value > 60)
                                {
                                    throw new ArgumentOutOfRangeException(nameof(end), "分钟和秒只能为00-60");
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("无效的值", nameof(begin));
                    }
                }
                if (DateTime.Parse(begin) >= DateTime.Parse(end))
                {
                    throw new ArgumentException("开始时段不能大于等于截止时段");
                }
                Begin = begin;
                End = end;
            }
        }
    }

    /// <summary>
    /// 优惠券
    /// </summary>
    public class CourtesyCard
    {

    }
}
