﻿namespace Tibia.Addresses
{
    public static class ContextMenus
    {
        /// <summary>
        /// The function used to add a context menu item.
        /// </summary>
        public static uint AddContextMenuPtr = 0x452BA0; //8.70

        /// <summary>
        /// The function used to process Tibia's "OnClick" events,
        /// that is, the events issued when you click a context
        /// menu item.
        /// </summary>
        public static uint OnClickContextMenuPtr = 0x44F760; //8.70

        /// <summary>
        /// The address in the virtual function table where the
        /// "OnClick" event for context menu items is.
        /// This address should be overwritten with a raw address,
        /// that is, the unmodified address of the function to be
        /// called when a "OnClick" event is issued.
        /// Moreover, it is HIGHLY (by highly I mean DO IT if you
        /// want to avoid sure trouble) recommended that you call
        /// OnClickContextMenuPtr from your hooked function to
        /// process standard Tibia events.
        /// </summary>
        public static uint OnClickContextMenuVf = 0x5BDB60; //8.70

        /// <summary>
        /// The "Set Outfit" context menu item function call.
        /// Overwrite it if you want to add a context menu item
        /// that is specific to your character
        /// </summary>
        public static uint AddSetOutfitContextMenu = 0x453AB0; //8.70

        /// <summary>
        /// The "Invite to Party" / "Leave Party" context menu
        /// item function call.
        /// Overwrite it if you want to add a context menu item
        /// that is specific to other players.
        /// </summary>
        public static uint AddPartyActionContextMenu = 0x4539E4; //8.70

        /// <summary>
        /// The "Copy Name" context menu item function call.
        /// Overwrite it if you want to add a context menu item
        /// that is specific to creatures (including you and monsters).
        /// </summary>
        public static uint AddCopyNameContextMenu = 0x453B4E; //8.70

        public static uint AddTradeWithContextMenu = 0x453749; //8.70

        /// <summary>
        /// The "Look" context menu item function call.
        /// Overwrite it if you want to add a context menu item
        /// that always appears on game window or inventory item clicks.
        /// </summary>
        public static uint AddLookContextMenu = 0x4535FF; //8.70

    }
}
