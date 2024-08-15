using System;

namespace DTO.v1_0;

public class UserWorkCreate
{
    public string WorkName { get; set; } = default!;
    public TimeSpan LunchBreakDuration { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
}