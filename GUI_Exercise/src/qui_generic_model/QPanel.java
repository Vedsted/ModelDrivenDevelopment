package qui_generic_model;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.util.Map;

/**
 * GUI metamodel: a nested panel organizing a number of nested components
 */
public class QPanel extends QComponent {
	/**
	 * Supported layouts
	 */
    public static enum Layout { FLOW, VERTICAL, HORIZONTAL; }
    /**
     * The layout of the panel
     */
    private Layout layout;
    /**
     * Create a panel model with the given layout
     * @param layout the layout of the panel
     */
    public QPanel(Layout layout) { this.layout = layout; }
    /**
     * Create JPanel containing the nested components
     */
    public JPanel create(ActionListener handler, Map<String,JComponent> componentMap) {
        JPanel panel = new JPanel();
        panel.setLayout(getLayout(panel));
        for(QComponent c: contents) panel.add(c.create(handler,componentMap));
        return panel;
    }
    /**
     * Create layout manager
     * @param panel the panel to assign the given layout manager
     * @return the new layout manager
     */
    private LayoutManager getLayout(JPanel panel) {
        if(layout== Layout.FLOW)
            return new FlowLayout();
        else if(layout== Layout.HORIZONTAL)
            return new BoxLayout(panel,BoxLayout.X_AXIS);
        else if(layout== Layout.VERTICAL)
            return new BoxLayout(panel,BoxLayout.Y_AXIS);
        else
            throw new Error("Unknown layout: "+layout);
    }
    /**
     * For debugging
     */
    @Override public String toString() {
        StringBuffer result = new StringBuffer("JPanel[");
        for(QComponent c: contents) result.append(c.toString()+",");
        result.append("]");
        return result.toString();
    }
}