﻿namespace CodeConnect.Features.Communities;

public class CommunityDto
{
    public int CommunityId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
}
