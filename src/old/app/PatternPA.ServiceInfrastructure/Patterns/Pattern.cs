namespace PatternPA.ServiceInfrastructure.Patterns
{
    public class Pattern
    {
        public Pattern(int lenght)
        {
            Lenght = lenght;
        }

        public int Lenght { get; set; }
        public string Value { get; set; }
    }
}