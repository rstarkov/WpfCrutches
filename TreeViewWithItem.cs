using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfCrutches
{
    /// <summary>
    /// Same as a TreeView, except that items which implement the <see cref="IHasTreeViewItem"/> interface will
    /// automatically receive the corresponding TreeViewItem container. This saves a lot of effort building simple
    /// trees, compared to the intended approach of having the ViewModel re-implement TreeViewItem features.
    /// (http://stackoverflow.com/questions/616948/)
    /// </summary>
    public class TreeViewWithItem : TreeView
    {
        /// <summary>Constructor.</summary>
        public TreeViewWithItem()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            var generator = sender as ItemContainerGenerator;
            if (generator.Status == GeneratorStatus.ContainersGenerated)
            {
                int i = 0;
                while (true)
                {
                    var container = generator.ContainerFromIndex(i);
                    if (container == null)
                        break;

                    var tvi = container as TreeViewItem;
                    if (tvi != null)
                        tvi.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

                    var item = generator.ItemFromContainer(container) as IHasTreeViewItem;
                    if (item != null)
                        item.TreeViewItem = tvi;

                    i++;
                }
            }
        }
    }

    /// <summary>
    /// Implemented by data models which are added to a TreeView with the intention of accessing the
    /// corresponding TreeViewItem directly.
    /// </summary>
    public interface IHasTreeViewItem
    {
        /// <summary>
        /// Gets the TreeViewItem associated with this item. This value will be null until the parent tree view item
        /// has been expanded (at least when using the HierarchicalDataTemplate).
        /// </summary>
        TreeViewItem TreeViewItem { get; set; }
    }
}
