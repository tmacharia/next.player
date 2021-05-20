namespace Tests.TestModels
{
    struct DateTimeFormat
    {
        public DateTimeFormat(string dateTime, string format)
        {
            DateTime = dateTime;
            Format = format;
        }
        public string Format { get; set; }
        public string DateTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1}", DateTime, Format);
        }
    }
}