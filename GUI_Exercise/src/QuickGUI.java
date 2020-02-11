/*
Copyright (c) 2012, Ulrik Pagh Schultz, University of Southern Denmark
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met: 

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer. 
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies, 
either expressed or implied, of the University of Southern Denmark.
*/

import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.JPanel;

import qui_generic_model.QButton;
import qui_generic_model.QComponent;
import qui_generic_model.QFrame;
import qui_generic_model.QLabel;
import qui_generic_model.QPanel;
import qui_generic_model.QPanel.Layout;

/**
 * QuickGUI allows concise description of a simple GUI (one frame, buttons, labels, organized into panels)
 * to be instantiated as a GUI that can be used from the application.
 * This version uses method sequence but only allows shallow GUIs to be built
 * @author ups
 */
public class QuickGUI {

    /**
     * Builder class: provides convenience methods for constructing a GUI metamodel
     * @author ups
     */
    public static abstract class GUIBuilder {
        /**
         * The top-level frame
         */
        private QFrame top;
        /**
         * The top-level panel
         */
        private QPanel panel;
        /**
         * The component currently being added to a row
         */
        private List<QComponent> currentRow = new ArrayList<QComponent>();
        /**
         * Create a frame with the given name and layout, containing the given nested model components,
         * and store it inside the builder
         * @param name title of the frame
         * @return the builder
         */
        public GUIBuilder frame(String name) {
            top = new QFrame(name);
            panel = new QPanel(Layout.VERTICAL);
            top.add(panel);
            return this; 
        }
        /**
         * Get the top-level panel into which all elements must be stored.
         * TODO: remove this method as you solve the assignment
         */
        public QPanel getTopPanel() {
        	return panel;
        }
        /**
         * Get the top-level panel into which all elements must be stored
         * TODO: remove this method as you solve the assignment
         */
        public List<QComponent> getCurrentRow() {
        	return currentRow;
        }
        /**
         * Flush the components stored in currentRow into a new panel
         * TODO: implement this method
         */
        public GUIBuilder newline() {
        	// ...
            QPanel p0 = new QPanel(Layout.HORIZONTAL);
            for(QComponent c: currentRow) p0.add(c);
            getTopPanel().add(p0);
            currentRow.clear();
        	return this;
        }
		/**
         * Create a label with the given properties
         * @param name the name of the label
         * @param text the text of the label
         * @return the builder
         * TODO: implement this method
         */
        public GUIBuilder label(String name, String text) { 
            // ...
            currentRow.add(new QLabel(name, text));
            return this;
        }
		/**
         * Create a label with identical name and text
         * @param name the name and text of the label
         * @return the builder
         */
        public GUIBuilder label(String text) { 
            return this.label(text,text);
        }
		/**
         * Create a button with the given properties
         * @param name the name of the button
         * @param text the text of the button
         * @return the builder
         * TODO: implement this method
         */
        public GUIBuilder button(String name, String text) { 
            // ...
            currentRow.add(new QButton(name, text));
            return this;
        }
		/**
         * Create a button with identical name and text
         * @param name the name and text of the button
         * @return the builder
         */
        public GUIBuilder button(String text) { 
            return this.button(text,text);
        }
        /**
         * Get the top-level frame
         * @return the top-level frame model
         */
        public QFrame getTop() { 
        	build(); 
        	return top; 
        }
        /**
         * Build a gui model, must have one and only one top-level frame
         */
        public abstract void build();
    }

    /**
     * Map from names to corresponding Swing components
     */
    private Map<String,JComponent> componentMap = new HashMap<String,JComponent>();
    
    /**
     * Instantiate a Swing GUI based on the given model
     * @param model the model to instantiate from
     * @param handler the handler to use when handling GUI events
     */
    public QuickGUI(final GUIBuilder model, final ActionListener handler) {
        javax.swing.SwingUtilities.invokeLater(new Runnable() {
            public void run() {
                buildGUI(model.getTop(),handler);
            }
        }); 
    }

    /**
     * Does the actual task of building the GUI for the given model and handler 
     * @param model to build the GUI from
     * @param handler for handling events
     */
    private void buildGUI(QFrame model, ActionListener handler) {
        // Create and set up the window.
        JFrame frame = new JFrame(model.getName());
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        // Content must be a panel
        QPanel panel = (QPanel)model.getContents().get(0);
        JPanel gui = panel.create(handler,componentMap);
        //Create and set up the content pane.
        frame.setContentPane(gui);
        gui.setOpaque(true);
         //Display the window.
        frame.pack();
        frame.setVisible(true);
    }
    
    /**
     * Get the component with a given name
     * @param name the name of the component to find
     * @return the component with matching name, if any, signalling an error otherwise
     */
    public JComponent getComponent(String name) {
        JComponent component = componentMap.get(name);
        if(component==null) throw new Error("Component not found: "+name);
        return component;
    }
}
