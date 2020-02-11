package qui_generic_model;

import javax.swing.*;
import java.awt.event.ActionListener;
import java.util.Map;

/**
 * GUI metamodel: a simple label
 */
public class QLabel extends QComponent {
    /**
     * The properties of this element
     */
    private String name, text;
    /**
     * Create a new Label element with the given properties
     * @param name the name of the button
     * @param text the label of the button
     */
    public QLabel(String name, String text) {
        this.name = name; this.text = text;
    }
    /**
     * Create the corresponding Swing element, depending on the kind, store in map etc
     */
    @Override protected JComponent create(ActionListener handler, Map<String,JComponent> componentMap) {
         JLabel label = new JLabel(text);
         if(name!=null) componentMap.put(name,label);
         return label;
    }
    /**
     * For debugging
     */
    @Override public String toString() {
        return "Label[...]";
    }
}