using System;
using System.Runtime.Serialization;

namespace Knapsack.ContainerPacking.Entities
{
    /// <summary>
    /// An item to be packed. Also used to hold post-packing details for the item.
    /// </summary>
    [DataContract]
	public class Item
	{
		#region Private Variables

		private decimal volume;

		#endregion Private Variables

		#region Constructors

        /// <summary>
        /// Initializes a new instance of the Item class.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <param name="length">The length of one of the three item dimensions.</param>
        /// <param name="width">The length of another of the three item dimensions.</param>
        /// <param name="height">The length of the other of the three item dimensions.</param>
        /// <param name="quantity">The item quantity.</param>
        public Item(int id, decimal length, decimal width, decimal height, int quantity,
            RotationType rotationType = RotationType.DepthHeightWidth, Position? position = default)
        {
            ID = id;
            Length = length;
            Width = width;
            Height = height;
            volume = length * width * height;
            Quantity = quantity;

            RotationType = rotationType;
            Position = position ?? Position.StartingPosition;
        }

        #endregion Constructors

		#region Public Properties

		/// <summary>
		/// Gets or sets the item ID.
		/// </summary>
		/// <value>
		/// The item ID.
		/// </value>
		[DataMember]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this item has already been packed.
		/// </summary>
		/// <value>
		///   True if the item has already been packed; otherwise, false.
		/// </value>
		[DataMember]
		public bool IsPacked { get; set; }

		/// <summary>
		/// Gets or sets the length of one of the item dimensions.
		/// </summary>
		/// <value>
		/// The first item dimension.
		/// </value>
		[DataMember]
		public decimal Length { get; set; }

		/// <summary>
		/// Gets or sets the length another of the item dimensions.
		/// </summary>
		/// <value>
		/// The second item dimension.
		/// </value>
		[DataMember]
		public decimal Width { get; set; }

		/// <summary>
		/// Gets or sets the third of the item dimensions.
		/// </summary>
		/// <value>
		/// The third item dimension.
		/// </value>
		[DataMember]
		public decimal Height { get; set; }

		/// <summary>
		/// Gets or sets the x coordinate of the location of the packed item within the container.
		/// </summary>
		/// <value>
		/// The x coordinate of the location of the packed item within the container.
		/// </value>
		[DataMember]
		public decimal CoordX { get; set; }

		/// <summary>
		/// Gets or sets the y coordinate of the location of the packed item within the container.
		/// </summary>
		/// <value>
		/// The y coordinate of the location of the packed item within the container.
		/// </value>
		[DataMember]
		public decimal CoordY { get; set; }

		/// <summary>
		/// Gets or sets the z coordinate of the location of the packed item within the container.
		/// </summary>
		/// <value>
		/// The z coordinate of the location of the packed item within the container.
		/// </value>
		[DataMember]
		public decimal CoordZ { get; set; }

		/// <summary>
		/// Gets or sets the item quantity.
		/// </summary>
		/// <value>
		/// The item quantity.
		/// </value>
		public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the x dimension of the orientation of the item as it has been packed.
		/// </summary>
		/// <value>
		/// The x dimension of the orientation of the item as it has been packed.
		/// </value>
		[DataMember]
		public decimal PackDimX { get; set; }

		/// <summary>
		/// Gets or sets the y dimension of the orientation of the item as it has been packed.
		/// </summary>
		/// <value>
		/// The y dimension of the orientation of the item as it has been packed.
		/// </value>
		[DataMember]
		public decimal PackDimY { get; set; }

		/// <summary>
		/// Gets or sets the z dimension of the orientation of the item as it has been packed.
		/// </summary>
		/// <value>
		/// The z dimension of the orientation of the item as it has been packed.
		/// </value>
		[DataMember]
		public decimal PackDimZ { get; set; }

		/// <summary>
		/// Gets the item volume.
		/// </summary>
		/// <value>
		/// The item volume.
		/// </value>
		[DataMember]
		public decimal Volume 
		{
			get
			{
				return volume;
			}
		}

        #endregion Public Properties

        #region Sharp3DPacking

        public RotationType RotationType { get; set; }

        public Position Position { get; set; }

        public override string ToString() =>
            $"{ID}({Length}x{Width}x{Height}) pos({Position}) rt({RotationType}:{(int)RotationType}) vol({Volume})";

        /// <summary>
        /// This method will map the dimensions (width, height, and depth) of the item to output variables
        /// based on the orientation (RotationType) of the item.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        internal void RotatedDimensions(out decimal width, out decimal height, out decimal depth)
        {
            switch (RotationType)
            {
                case RotationType.WidthHeightDepth:
                    width = Width;
                    height = Height;
                    depth = Length;

                    return;

                case RotationType.HeightWidthDepth:
                    width = Height;
                    height = Width;
                    depth = Length;

                    return;

                case RotationType.HeightDepthWidth:
                    width = Height;
                    height = Length;
                    depth = Width;

                    return;

                case RotationType.DepthHeightWidth:
                    width = Length;
                    height = Height;
                    depth = Width;

                    return;

                case RotationType.DepthWidthHeight:
                    width = Length;
                    height = Width;
                    depth = Height;

                    return;

                case RotationType.WidthDepthHeight:
                    width = Width;
                    height = Length;
                    depth = Height;

                    return;

                default:
                    width = 0;
                    height = 0;
                    depth = 0;

                    return;
            }
        }

        /// <summary>
        /// Returns the position of an item when the item is rotated by
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        internal Position RotatePosition(Axis axis)
        {
            RotatedDimensions(out var width, out var height, out var depth);

            switch (axis)
            {
                case Axis.Width:
                    return new Position(Position.X + width, Position.Y, Position.Z);
                case Axis.Height:
                    return new Position(Position.X, Position.Y + height, Position.Z);
                case Axis.Depth:
                    return new Position(Position.X, Position.Y, Position.Z + depth);
                default:
                    return Position.StartingPosition;
            }
        }

        internal decimal PositionAtAxis(Axis axis)
        {
            switch (axis)
            {
                case Axis.Width:
                    return Position.X;
                case Axis.Height:
                    return Position.Y;
                case Axis.Depth:
                    return Position.Z;
                default:
                    return default;
            }
        }

        internal decimal RotatedPositionAtAxis(Axis axis)
        {
            RotatedDimensions(out var width, out var height, out var depth);

            switch (axis)
            {
                case Axis.Width:
                    return width;
                case Axis.Height:
                    return height;
                case Axis.Depth:
                    return depth;
                default:
                    return default;
            }
        }

        public bool IntersectsWith(Item item) =>
            DoRectanglesIntersect(this, item, Axis.Width, Axis.Height) &&
            DoRectanglesIntersect(this, item, Axis.Height, Axis.Depth) &&
            DoRectanglesIntersect(this, item, Axis.Width, Axis.Depth);

        internal static bool DoRectanglesIntersect(Item item1, Item item2, Axis axis1, Axis axis2)
        {
            var d1a1 = item1.RotatedPositionAtAxis(axis1);
            var cx1 = item1.PositionAtAxis(axis1) + d1a1 / 2;
            var d1a2 = item1.RotatedPositionAtAxis(axis2);
            var cy1 = item1.PositionAtAxis(axis2) + d1a2 / 2;

            var d2a1 = item2.RotatedPositionAtAxis(axis1);
            var cx2 = item2.PositionAtAxis(axis1) + d2a1 / 2;
            var d2a2 = item2.RotatedPositionAtAxis(axis2);
            var cy2 = item2.PositionAtAxis(axis2) + d2a2 / 2;

            var ix = Math.Max(cx1, cx2) - Math.Min(cx1, cx2);
            var iy = Math.Max(cy1, cy2) - Math.Min(cy1, cy2);

            return ix < (d1a1 + d2a1) / 2 && iy < (d1a2 + d2a2) / 2;
        }

        #endregion
    }
}
