using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Enum
{
    /// <summary>
    /// 用卡环节
    /// </summary>
    public class HealthCardYKHJEnum
    {
        /// <summary>
        /// 挂号
        /// </summary>
        public enum GHEnum
        {
            /// <summary>
            /// 预约挂号
            /// </summary>
            [Description("预约挂号")]
            YYGH = 0101011,
            /// <summary>
            /// 当日挂号
            /// </summary>
            [Description("当日挂号")]
            DRGH = 0101012,
            /// <summary>
            /// 挂号记录
            /// </summary>
            [Description("挂号记录")]
            GHJL = 0101013,
            /// <summary>
            /// 预约导诊
            /// </summary>
            [Description("预约导诊")]
            YYDZ = 0101014,
        }
    }

    /// <summary>
    /// 用卡渠道
    /// </summary>
    public enum HealthCardYKQDEnum
    {
        /// <summary>
        /// 人工窗口
        /// </summary>
        [Description("人工窗口")]
        RGCK = 0100,
        /// <summary>
        /// 人工窗口
        /// </summary>
        [Description("自助机")]
        ZZJ = 0200,
        /// <summary>
        /// 人工窗口
        /// </summary>
        [Description("医生工作台")]
        YSGZT = 0300,
        /// <summary>
        /// 人工窗口
        /// </summary>
        [Description("服务号")]
        FWH = 0401,
        /// <summary>
        /// 人工窗口
        /// </summary>
        [Description("小程序")]
        XCX = 0402,
    }
}
