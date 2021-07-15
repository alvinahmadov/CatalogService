using System;
using System.Collections.Generic;

namespace Catalog.Client
{
	public class ControlContainer
	{
		public Int32 Id { get; private set; }

		public Boolean IsTopControl { get; set; }

		public Boolean IsCurrentControl { get; set; }

		public BaseGridControl Control { get; private set; }

		public ControlContainer(Int32 id, BaseGridControl control)
		{
			Id = id;
			Control = control;
			IsTopControl = false;
			IsCurrentControl = false;
		}

		public ControlContainer(Int32 id, BaseGridControl control, Boolean isTop, Boolean isCurrent)
		{
			Id = id;
			Control = control;
			IsTopControl = isTop;
			IsCurrentControl = isCurrent;
		}
	}

	public class ParentControlContainer : ControlContainer
	{
		public List<ChildControlContainer> Children { get; private set; }

		public ParentControlContainer(Int32 id, BaseGridControl control)
			: base(id, control, true, false)
		{
			Children = new List<ChildControlContainer>();
		}

		public Boolean Add(Int32 childId, BaseGridControl childControl)
		{
			Boolean hasChild = this[childId] != null;
			if (!hasChild)
				Children.Add(new ChildControlContainer(childId, this.Id, childControl));

			return !hasChild;
		}

		public ChildControlContainer this[int childId]
		{
			get
			{
				ChildControlContainer control = null;
				foreach (var child in Children)
				{
					if (childId == child.Id)
					{
						control = child;
						break;
					}
				}

				return control;
			}
		}
	}

	public class ChildControlContainer : ControlContainer
	{
		public Int32 ParentId { get; set; }

		public ChildControlContainer(Int32 id, Int32 parentId, BaseGridControl control)
			: base(id, control, false, false)
		{
			ParentId = parentId;
		}
	}

	public class GridControlCollection
	{
		public List<ControlContainer> Controls { get; private set; }

		public GridControlCollection()
		{
			Controls = new List<ControlContainer>();
		}

		public ControlContainer ActiveControl
		{
			get; set;
		}

		public void AddControl(BaseGridControl control, ProductTag tag)
		{
			// Add common control which is not restricted by category or subcategory
			// Or add control, which is restricted only by the category
			if (tag.IsAbsolute)
			{
				var container = new ControlContainer(tag.CID, control);
				Controls.Add(container);
				ActiveControl = container;
			}
			else if (tag.IsZero || tag.SID == -1)
			{
				if (!Controls.Exists(c => c.Id == tag.CID))
				{
					var container = new ParentControlContainer(tag.CID, control);
					Controls.Add(container);
					ActiveControl = container;
				}
			}
			// Add distinct product control restricted by category and subcategory
			else
			{
				var parentHolder = Controls[tag.CID] as ParentControlContainer;
				parentHolder.Add(tag.SID, control);
				ActiveControl = parentHolder[tag.SID];
			}
		}

		public ControlContainer this[int parentId]
		{
			get => this.Controls[parentId];
		}

		public ControlContainer Get(Int32 parentId, Int32 childId)
		{
			var ctrl = Controls.Find(c => c.Id == parentId) as ParentControlContainer;
			return ctrl[childId];
		}

		public ControlContainer Find(Int32 parentId, Int32 childId)
		{
			var parentCtrl = Controls.Find(c => c.Id == parentId) as ParentControlContainer;
			ChildControlContainer childCtrl = null;
			if (parentCtrl != null)
				childCtrl = parentCtrl[childId];
			else
				return null;

			if (childCtrl == null)
				return parentCtrl;
			else
				return childCtrl;
		}
	}
}