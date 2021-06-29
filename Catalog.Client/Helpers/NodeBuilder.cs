using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Catalog.Client.Properties;
using Catalog.Common.Repository;
using Catalog.Common.Utils;
using Telerik.WinControls.UI;


using TreeNodeList = System.Collections.Generic.SortedList<string, Telerik.WinControls.UI.RadTreeNode>;

namespace Catalog.Client
{
	public class ProductTag
	{
		public int Count { get; set; }

		public String Name { get; set; }

		public int CID { get; private set; }

		public int SID { get; private set; }

		public bool IsZero
		{
			get => this.CID == 0 && this.SID == 0;
		}

		public bool IsAbsolute
		{
			get => this.CID == Int32.MinValue && this.SID == Int32.MinValue;
		}

		public ProductTag(int cid, int sid)
		{
			this.CID = cid;
			this.SID = sid;
			Count = 0;
		}

		public ProductTag() : this(0, 0)
		{ }

		public ProductTag(int cid) : this(cid, -1)
		{ }

		public override String ToString()
		{
			if (SID > 0)
				return $"<html>{Name}<span style=\"font-size:8pt;\"> ({Count})</span>";

			return $"<html><b>{Name}<span style=\"font-size:10pt;\"> ({Count})</span></b>";
		}

		public static ProductTag Absolute
		{
			get => new ProductTag(Int32.MinValue, Int32.MinValue);
		}

		public static ProductTag Zero
		{
			get => new ProductTag(0, 0);
		}

		public override Boolean Equals(Object obj)
		{
			var productTag = obj as ProductTag;
			return this.CID == productTag.CID && this.SID == productTag.SID;
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public sealed class NodeBuilder
	{
		public FontStyle TreeFontStyle { get; set; }

		public String TreeFontName { get; set; }

		public float TreeFontSize { get; set; }

		private TreeNodeList TreeNodes { get; set; } = new TreeNodeList();

		private Action<ProductTag> Callback { get; set; }

		public NodeBuilder(RadTreeView treeView)
			: this(treeView, FontStyle.Regular) { }

		public NodeBuilder(RadTreeView treeView, FontStyle fontStyle)
			: this(treeView, "Arial", 12F, fontStyle)
		{ }

		public NodeBuilder(RadTreeView treeView, String fontName, FontStyle fontStyle)
			: this(treeView, fontName, 12F, fontStyle)
		{ }

		public NodeBuilder(
			RadTreeView treeView,
			String fontName,
			float fontSize,
			FontStyle fontStyle
		)
		{
			TreeFontName = fontName;
			TreeFontSize = fontSize;
			TreeFontStyle = fontStyle;
		}

		public TreeNodeList
		InitializeNodes(bool expanded, string defaultNodeName, Action<ProductTag> callback)
		{
			this.Callback = callback;
			var categories = Repository.Context.ProductCategories
			.Include("ProductSubcategories");
			//.Expand(p => p.ProductSubcategories);
			// Common products
			var commonTag = ProductTag.Zero;
			commonTag.Count = MainRepository.ProductInventoriesCache.Count;
			commonTag.Name = defaultNodeName;

			AddNode(commonTag, "_All", commonTag.ToString(), expanded);

			int index = 0;
			foreach (var category in categories)
			{
				//Subcategory
				var radTreeNodes = new List<RadTreeNode>();
				var categoryTag = new ProductTag(category.ProductCategoryID)
				{
					Name = category.Name
				};

				foreach (var subcat in category.ProductSubcategories)
				{
					var tag = new ProductTag(subcat.ProductCategoryID, subcat.ProductSubcategoryID)
					{
						Name = subcat.Name
					};

					tag.Count = MainRepository.ProductInventoriesCache
												.Count(p => p.ProductSubcategoryID == tag.SID);

					radTreeNodes.Add(new RadTreeNode
					{
						Tag = tag,
						Name = subcat.Name.ToLower(),
						Text = tag.ToString(),
						ToolTipText = tag.Count.ToString(),
						Expanded = expanded
					});
					categoryTag.Count += tag.Count;
					
				}
				++index;
				// Category
				AddNode(categoryTag, category.Name, categoryTag.ToString(), radTreeNodes, expanded);
			}
			return TreeNodes;
		}

		private RadTreeNode AddNode(ProductTag tag, string name, string value, bool expanded)
		{
			return AddNode(tag, name, value, null, expanded);
		}

		private RadTreeNode AddNode(
			ProductTag tag,
			string name,
			string value,
			List<RadTreeNode> children,
			bool expanded
		)
		{
			RadTreeNode topNode;
			Callback(tag);
			if (children != null)
			{

				children.ForEach(node => Callback(node.Tag as ProductTag));
				topNode = new RadTreeNode(name, children.ToArray())
				{
					Tag = tag,
					Text = value,
					Expanded = expanded,
				};
			}
			else
			{
				topNode = new RadTreeNode(name, expanded)
				{
					Tag = tag,
					Text = value,
				};
			}
			topNode.Style.Font = new Font(TreeFontName, TreeFontSize);
			TreeNodes.Add(name, topNode);
			return topNode;
		}
	}
}
