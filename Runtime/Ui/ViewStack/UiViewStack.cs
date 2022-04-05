﻿using Juce.Core.Repositories;
using Juce.Core.Sequencing;
using Juce.CoreUnity.Ui.Frame;
using Juce.CoreUnity.ViewStack.Context;
using Juce.CoreUnity.ViewStack.Entries;
using Juce.CoreUnity.ViewStack.Builder;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Juce.CoreUnity.ViewStack
{
    public class UiViewStack : IUiViewStack
    {
        private readonly IKeyValueRepository<Type, IViewStackEntry> entriesRepository = new SimpleKeyValueRepository<Type, IViewStackEntry>();
        private readonly ISingleRepository<IViewContext> currentContextRepository = new SimpleSingleRepository<IViewContext>();
        private readonly Stack<Type> viewStackQueue = new Stack<Type>();
        private readonly ISequencer sequencer = new Sequencer();

        private readonly IUiFrame frame;

        public UiViewStack(IUiFrame frame)
        {
            this.frame = frame;
        }

        public void Register(IViewStackEntry entry)
        {
            bool idAlreadyAdded = entriesRepository.TryGet(entry.Id, out _);

            if(idAlreadyAdded)
            {
                UnityEngine.Debug.LogError($"{nameof(IViewStackEntry)} with id {entry.Id} already registered");
                return;
            }

            entry.Visible.SetVisible(visible: true, instantly: true, CancellationToken.None);
            entry.Visible.SetVisible(visible: false, instantly: true, CancellationToken.None);

            entriesRepository.Add(entry.Id, entry);

            frame.Register(entry.Transform);
        }

        public void Unregister(IViewStackEntry entry)
        {
            entriesRepository.Remove(entry.Id);

            frame.Unregister(entry.Transform);
        }

        public IViewStackSequenceBuilder New()
        {
            return new ViewStackSequenceBuilder(
                frame,
                entriesRepository,
                currentContextRepository,
                viewStackQueue,
                sequencer
                );
        }
    }
}
