using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

internal class Boundary
{
    internal readonly Lazy<Segment[]> segments;
    
    // order of points is critical
    internal Boundary(Lazy<Chromaticity[]> points)
    {
        segments = new Lazy<Segment[]>(() =>
        {
            var result = new List<Segment>();
            for (var i = 0; i < points.Value.Length; i++)
            {
                var start = points.Value[i];
                var end = points.Value[(i + 1) % points.Value.Length];
                var segment = new Segment(start, end);
                result.Add(segment);
            }

            return result.ToArray();
        });
    }
    
    internal bool Contains(Chromaticity sample)
    {
        var reference = new Chromaticity(sample.X, sample.Y + 0.00001);
        var intersects = GetIntersects(sample, reference);

        // no intersects; sample is definitely outside the boundary
        if (intersects == null) return false;

        var near = intersects.Value.near.Point;
        var far = intersects.Value.far.Point;
        
        // two distinct intersects; if sample is between them, it is inside the boundary
        if (far != near) return IsBetween(sample.Y, near.Y, far.Y);
        
        // single intersect; if sample is the intersect itself, it is inside the boundary
        // - the intersecting line meets ONLY at a boundary node
        // - all other points on the intersecting line are outside the boundary 
        var sameX = (sample.X - near.X).IsEffectivelyZero();
        var sameY = (sample.Y - near.Y).IsEffectivelyZero();
        return sameX && sameY;
    }
    
    internal (Intersect near, Intersect far)? GetIntersects(Chromaticity sample, Chromaticity reference)
    {
        var intersects = FindIntersects(sample, reference);
        if (intersects == NoIntersects) return null;

        var (nearIntersects, farIntersects) = intersects;
        
        var near = nearIntersects.Last();
        var far = !farIntersects.Any() ? near : farIntersects.Last();
        return (near, far);
    }
    
    private static readonly (Intersect[] near, Intersect[] far) NoIntersects = (Array.Empty<Intersect>(), Array.Empty<Intersect>());
    private (Intersect[] near, Intersect[] far) FindIntersects(Chromaticity sample, Chromaticity reference)
    {
        if (sample == reference) return NoIntersects;
        
        var intersects = segments.Value
            .Select(segment => new Intersect(segment, sample, reference))
            .Where(intersect => intersect.IsOnSegment && !double.IsInfinity(intersect.DistanceFromSample))
            .ToArray();

        if (!intersects.Any()) return NoIntersects;
        
        var minDistanceFromSample = intersects.Min(x => x.DistanceFromSample);
        var nearIntersects = intersects.Where(x => (x.DistanceFromSample - minDistanceFromSample).IsEffectivelyZero()).ToArray();
        var farIntersects = intersects.Except(nearIntersects).ToArray();
        
        // this step is only used by the spectral boundary implementation
        // to remove line of purples when converging at min or max wavelength
        (nearIntersects, farIntersects) = FilterIntersects(nearIntersects, farIntersects);

        // if intersects are equidistant in different directions, they are all equally near
        // e.g. a square boundary with the sample in the centre and reference directly above the sample
        // there will be 2 near and 0 far intersects (top and bottom segments are the same distance from sample)
        // in this case, just choose one intersect to be the far one
        if (!farIntersects.Any() && nearIntersects.Length > 1)
        {
            farIntersects = new[] { nearIntersects.Last() };
            nearIntersects = nearIntersects.Except(farIntersects).ToArray();
        }
        
        return (nearIntersects, farIntersects);
    }
    
    protected virtual (Intersect[] nearIntersects, Intersect[] farIntersects) FilterIntersects(Intersect[] nearIntersects, Intersect[] farIntersects)
    {
        return (nearIntersects, farIntersects);
    }
    
    private static bool IsBetween(double value, double near, double far)
    {
        var lower = Math.Min(near, far);
        var upper = Math.Max(near, far);
        var isAboveLower = value > lower || (value - lower).IsEffectivelyZero();
        var isBelowUpper = value < upper || (upper - value).IsEffectivelyZero();
        return isAboveLower && isBelowUpper;
    }
    
    public override string ToString() => $"{segments.Value.Length} segments";
}

internal record Intersect(Segment Segment, Chromaticity Sample, Chromaticity Reference)
{
    internal Segment Segment { get; } = Segment;
    internal Chromaticity Sample { get; } = Sample;
    internal Chromaticity Reference { get; } = Reference;
    internal Segment ReferenceToSample { get; } = new(Reference, Sample);
    internal Chromaticity Point => ReferenceToSample.Line.GetIntersect(Segment.Line);
    
    internal Segment SampleToIntersect => new(Sample, Point);
    internal double DistanceFromSample => SampleToIntersect.Length;
    
    internal Segment ReferenceToIntersect => new(Reference, Point);
    internal double DistanceFromReference => ReferenceToIntersect.Length;
    
    internal double DistanceFromStart => Distance(Point, Segment.Start);
    internal double DistanceFromEnd => Distance(Point, Segment.End);
    internal double SegmentLengthViaIntersect => DistanceFromStart + DistanceFromEnd;
    internal double SegmentLengthDifference => Math.Abs(SegmentLengthViaIntersect - Segment.Length); // the closer to zero, the closer it lies on the segment
    internal bool IsOnSegment => SegmentLengthDifference.IsEffectivelyZero();

    public override string ToString() => $"{Segment} at {Point} · on segment: {IsOnSegment} · {DistanceFromStart / Segment.Length * 100:F0}% from start · {DistanceFromSample:F2} from sample";
}