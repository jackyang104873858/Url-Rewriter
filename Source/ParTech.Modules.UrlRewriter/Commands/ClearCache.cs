﻿using ParTech.Modules.UrlRewriter.Events;
using ParTech.Modules.UrlRewriter.Pipelines;
using Sitecore;
using Sitecore.Events;
using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace ParTech.Modules.UrlRewriter.Commands
{
    /// <summary>
    /// Clear the URL Rewriter cache by adding a ClearCacheEvent to the EventQueue.
    /// </summary>
    public class ClearCache : Command
    {
        /// <summary>
        /// Executes the command in the specified context.
        /// </summary>
        /// <param name="context"></param>
        public override void Execute(CommandContext context)
        {
            Context.ClientPage.Start(this, "Confirm", new ClientPipelineArgs());
        }

        /// <summary>
        /// Adds the ClearCacheEvent to the EventQueue after users confirmation.
        /// </summary>
        /// <param name="args"></param>
        public void Confirm(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {
                SheerResponse.Confirm("Are you sure you want to clear the URL Rewriter cache?");
                args.WaitForPostBack();
            }
            else if (args.Result == "yes")
            {
                // Raise the clearcache event so the cache is cleared on the local instance.
                Event.RaiseEvent("urlrewriter:clearcache", new ClearCacheEventArgs(new ClearCacheEvent()));

                // Add a ClearCacheEvent to the EventQueue to clear the cache on all instances.
                Sitecore.Eventing.EventManager.QueueEvent<ClearCacheEvent>(new ClearCacheEvent(), true, true);
            }
        }
    }
}