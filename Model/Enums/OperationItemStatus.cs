
using System.Runtime.Serialization;

namespace Maximiz.Model.Enums
{

    /// <summary>
    /// Indicates the status of one (or multiple) <see cref="Entity.Entity"/>  
    /// within an <see cref="Operations.Operation"/>.
    /// </summary>
    public enum OperationItemStatus
    {

        /// <summary>
        /// An item is awaiting processing, but has been claimed by some <see cref="Operations.Operation"/>.
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>
        /// An item is being processed within some <see cref="Operations.Operation"/>.
        /// </summary>
        [EnumMember(Value = "processing")]
        Processing,

        /// <summary>
        /// An item is up to date.
        /// </summary>
        [EnumMember(Value = "up_to_date")]
        UpToDate,

        /// <summary>
        /// An exception was encountered when processing this item.
        /// </summary>
        [EnumMember(Value = "exception")]
        Exception,

        /// <summary>
        /// A timeout was encountered when processing this item.
        /// </summary>
        [EnumMember(Value = "timeout")]
        Timeout,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "pending_restore")]
        PendingRestore,

        /// <summary>
        /// This item is being rolled back.
        /// </summary>
        [EnumMember(Value = "rolling_back")]
        RollingBack,

        /// <summary>
        /// This item is currently in an <see cref="Operations.Operation"/>.
        /// </summary>
        [EnumMember(Value = "in_operation")]
        InOperation,

        /// <summary>
        /// This item is ready to be deleted from our data store.
        /// </summary>
        [EnumMember(Value = "delete_ready")]
        DeleteReady,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "pending_create")]
        PendingCreate,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "pending_update")]
        PendingUpdate,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "pending_delete")]
        PendingDelete,

    }
}
