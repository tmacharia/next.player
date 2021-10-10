using System.ComponentModel;

namespace Next.PCL.Enums
{
    /// <summary>
    /// Maturity Ratings, Motion Picture Association.
    /// </summary>
    public enum MPAA
    {
        /// <summary>
        /// General Audiences.<br/>
        /// All Ages Admitted
        /// </summary>
        G = 0X0,
        /// <summary>
        /// Parental Guidance Suggested.<br/>
        /// Some material may not be suitable for children.
        /// </summary>
        PG = 0X07,
        /// <summary>
        /// Parents Strongly Cautioned.<br/>
        /// Some material may be inappropriate for children under 13.
        /// </summary>
        [Description("PG-13")] PG_13 = 0X013,
        /// <summary>
        /// Young Adults(16+), PG-16
        /// </summary>
        [Description("PG-16")] PG_16 = 0X016,
        /// <summary>
        /// No One 17 &amp; Under Admitted.
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
        /// <summary>
        /// Restricted.<br/>
        /// Under 17 requires accompanying parent or adult guardian.
        /// </summary>
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