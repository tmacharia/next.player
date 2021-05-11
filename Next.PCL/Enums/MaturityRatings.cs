using System.ComponentModel;

namespace Next.PCL.Entities
{
    /// <summary>
    /// Movies Maturity Ratings
    /// </summary>
    public enum MPAA
    {
        /// <summary>
        /// Kids(All), GE
        /// </summary>
        G = 0X0,
        /// <summary>
        /// Older Kids(7+), PG
        /// </summary>
        PG = 0X07,
        /// <summary>
        /// Teens(13+), PG-13
        /// </summary>
        [Description("PG-13")] PG_13 = 0X013,
        /// <summary>
        /// Young Adults(16+), PG-16
        /// </summary>
        [Description("PG-16")] PG_16 = 0X016,
        /// <summary>
        /// Adults(18+), NC-17
        /// </summary>
        [Description("NC-17")] NC_17 = 0X0180,
        /// <summary>
        /// Adults(18+), NR
        /// </summary>
        NR = 0X0181,
        /// <summary>
        /// Adults(18+), Unrated
        /// </summary>
        Unrated = 0X0182,
        R = 0X0183
    }
    public enum TVAA
    {
        /// <summary>
        /// Kids(All), TV-G
        /// </summary>
        [Description("TV-G")] TV_G = 0X100,
        /// <summary>
        /// Kids(All), TV-Y
        /// </summary>
        [Description("TV-Y")] TV_Y = 0X101,
        /// <summary>
        /// Older Kids(7+), TV-Y7
        /// </summary>
        [Description("TV-Y7")] TV_Y7 = 0X170,
        /// <summary>
        /// Older Kids(7+), TV-Y7-FV
        /// </summary>
        [Description("TV-Y7-FV")] TV_Y7_FV = 0X171,
        /// <summary>
        /// Older Kids(7+), TV-PG
        /// </summary>
        [Description("TV-PG")] TV_PG = 0X172,
        /// <summary>
        /// Young Adults(16+), TV-14
        /// </summary>
        [Description("TV-14")] TV_14 = 0X114,
        /// <summary>
        /// Adults(18+), TV-MA
        /// </summary>
        [Description("TV-MA")] TV_MA = 0X118
    }
}