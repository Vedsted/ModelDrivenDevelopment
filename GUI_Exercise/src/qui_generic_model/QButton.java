package qui_generic_model;

import javax.swing.*;
import java.awt.event.ActionListener;
import java.util.Map;

/**
 * GUI metamodel: a simple, named button
 */
public class QButton extends QComponent {
    /**
     * The properties of this element
     */
    private String name, text;
    /**
     * Create a new Button element with the given properties
     * @param name the name of the button
     * @param text the text of the button
     */
    public QButton(String name, String text) {
        this.name = name; this.text = text;
    }
    /**
     * Create the corresponding Swing element, depending on the kind, store in map etc
     */
    @Override protected JComponent create(ActionListener handler, Map<String,JComponent> componentMap) {
        JButton button = new JButton(text);
        button.setActionCommand(name);
        button.addActionListener(handler);
        if(name!=null) componentMap.put(name, button);
        return button;
    }
    /**
     * For debugging
     */
    @Override public String toString() {
        return "Button[...]";
    }
}