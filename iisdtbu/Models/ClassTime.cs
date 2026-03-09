namespace iisdtbu.Models
{
    /// <summary>
    /// 上课时间表配置
    /// 支持不同教学楼的不同时间安排
    /// </summary>
    public static class ClassTime
    {
        /// <summary>
        /// 教学楼类型
        /// </summary>
        public enum BuildingType
        {
            /// <summary>
            /// 东校：第二教学楼、第四教学楼、综合楼、行政楼、商学实验中心、室内外体育课
            /// 西校：第三教学楼、工学实验中心、室内外体育课
            /// </summary>
            TypeA,

            /// <summary>
            /// 东校：第三教学楼、第五教学楼
            /// 西校：第一教学楼、第二教学楼
            /// </summary>
            TypeB
        }

        /// <summary>
        /// 节次类型
        /// </summary>
        public enum PeriodType
        {
            第1_2节,
            课间,
            第3_4节,
            第5_6节,
            第7_8节,
            第9_10节
        }

        /// <summary>
        /// 时间段信息
        /// </summary>
        public class TimeSlot
        {
            /// <summary>
            /// 开始时间 (例如: "8:00")
            /// </summary>
            public string StartTime { get; set; } = string.Empty;

            /// <summary>
            /// 结束时间 (例如: "9:30")
            /// </summary>
            public string EndTime { get; set; } = string.Empty;

            /// <summary>
            /// 时间段描述
            /// </summary>
            public string DisplayTime => $"{StartTime}-{EndTime}";
        }

        /// <summary>
        /// 获取指定教学楼类型和节次的时间安排
        /// </summary>
        /// <param name="buildingType">教学楼类型</param>
        /// <param name="period">节次</param>
        /// <returns>时间段信息</returns>
        public static TimeSlot GetTimeSlot(