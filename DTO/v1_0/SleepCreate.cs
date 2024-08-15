using System;

namespace DTO.v1_0;

public class SleepCreate
{
    public DateOnly Day { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}