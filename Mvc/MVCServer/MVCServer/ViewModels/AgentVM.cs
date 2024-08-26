﻿// Ignore Spelling: Dto

namespace MVCServer.ViewModels
{
    public enum AgentStatus
    {
        Active,
        Inactive
    }

    public class AgentVM
    {
        public string? Token { get; set; }
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public AgentStatus Status { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string? PhotoUrl { get; set; }
        public int Eliminations { get; set; }
    }
}
