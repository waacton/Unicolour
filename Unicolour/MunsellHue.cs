namespace Wacton.Unicolour;

    public record MunsellHue
    {
        public double Number { get; }
        public string Letter { get; }
        internal double Degrees { get; }

        internal MunsellHue(double number, string letter)
        {
            number = number.Clamp(0, 10);
            letter = letter.Trim().ToUpper();
            Degrees = ToDegrees(number, letter);
            Number = number;
            Letter = letter;
        }

        public MunsellHue(double degrees)
        {
            Degrees = degrees;
            var (number, letter) = double.IsNaN(Degrees) ? (double.NaN, "NaN") : FromDegrees(Degrees);
            Number = number;
            Letter = letter;
        }
        
        // this is only used by the table of radially interpolated hue segments
        // which are defined anti-clockwise as red -> blue is depicted on a chromaticity diagram
        // in terms of traditional hue degrees, this means the end is defined first
        internal bool IsBetween((double number, string letter) end, (double number, string letter) start)
        {
            return IsBetween(new MunsellHue(end.number, end.letter), new MunsellHue(start.number, start.letter));
        }
        
        private bool IsBetween(MunsellHue end, MunsellHue start)
        {
            var adapted = Wacton.Unicolour.Hue.Unwrap(end.Degrees, start.Degrees);
            var min = Math.Min(adapted.start, adapted.end);
            var max = Math.Max(adapted.start, adapted.end);
            return Degrees >= min && Degrees <= max || Degrees + 360 >= min && Degrees + 360 <= max;
        }
        
        private static readonly string[] Hues = { "R", "YR", "Y", "GY", "G", "BG", "B", "PB", "P", "RP" };
        
        internal static double ToDegrees(double hueNumber, string hueLetter)
        {
            var bandIndex = Array.IndexOf(Hues, hueLetter);
            if (bandIndex == -1) return double.NaN;
            
            var minDegrees = bandIndex * Node.DegreesPerHueLetter;
            var maxDegrees = (bandIndex + 1) * Node.DegreesPerHueLetter;
            var distance = hueNumber / 10.0; // maps 0 - 10 to 0 - 1
            var baseDegrees = Interpolation.Linear(minDegrees, maxDegrees, distance);
            // var degrees = baseDegrees - 2 * Node.DegreesPerHueNumber; // shifts degrees so 5R is 0 instead of 0R / 10RP
            return baseDegrees.Modulo(360);
        }

        internal static (double number, string letter) FromDegrees(double degrees)
        {
            // degrees += 2 * Node.DegreesPerHueNumber; // shifts degrees so 0R is 0 instead of 5R
            degrees = degrees.Modulo(360);
            var bandLocation = degrees / Node.DegreesPerHueLetter;
            var bandIndex = (int)Math.Truncate(bandLocation);
            var hueLetter = Hues[bandIndex];
            var hueNumber = (bandLocation - bandIndex) * 10;
            if (hueNumber != 0) return (hueNumber, hueLetter);
        
            bandIndex = bandIndex == 0 ? Hues.Length - 1 : bandIndex - 1;
            hueLetter = Hues[bandIndex];
            hueNumber = 10;
            return (hueNumber, hueLetter);
        }
        
        public override string ToString() => double.IsNaN(Degrees) ? $"{double.NaN}" : $"{Number}{Letter} ({Degrees}°)";
}