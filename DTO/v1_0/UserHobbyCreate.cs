using System;

namespace DTO.v1_0;

public class UserHobbyCreate
{
    public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;
    public string HobbyName { get; set; } = default!;
    public string? Language { get; set; }
}