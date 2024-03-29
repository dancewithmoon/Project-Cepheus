﻿using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService, IReadonlyProgressService
    {
        public PlayerProgress Progress { get; set; }
        public IReadOnlyPlayerProgress ProgressReadonly => Progress;
    }
}