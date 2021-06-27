package manager;

import com.microsoft.azure.sdk.iot.service.Configuration;
import com.microsoft.azure.sdk.iot.service.ConfigurationContent;
import com.microsoft.azure.sdk.iot.service.RegistryManager;
import com.microsoft.azure.sdk.iot.service.devicetwin.DeviceTwin;
import com.microsoft.azure.sdk.iot.service.devicetwin.DeviceTwinDevice;
import com.microsoft.azure.sdk.iot.service.devicetwin.Pair;
import com.microsoft.azure.sdk.iot.service.devicetwin.Query;
import com.microsoft.azure.sdk.iot.service.exceptions.IotHubException;
import manager.azure.NewVersion;
import manager.azure.WeatherDataPatch;
import manager.properties.PropertyFileReader;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.IOException;
import java.util.Collections;
import java.util.HashSet;
import java.util.Set;
import java.util.UUID;


public class SimulatedDeviceManager {

    public static void main(String[] args) {
        final Logger logger = LogManager.getLogger(SimulatedDeviceManager.class);
        logger.info("Starting the Simulated Device Manager");
        new ManagerForm();
    }

    private static class ManagerForm {

        DefaultListModel deviceListSource;
        DeviceTwin twinClient;
        RegistryManager registryManager;
        final Logger logger;

        public ManagerForm() {
            logger = LogManager.getLogger(ManagerForm.class);
            try {
                final PropertyFileReader properties = new PropertyFileReader();
                final String connectionString = properties.getProperty("connectionString");
                twinClient = DeviceTwin.createFromConnectionString(connectionString);
                registryManager = RegistryManager.createFromConnectionString(connectionString);
            } catch (IOException e) {
                twinClient = null;
                logger.error(e);
            }

            JFrame frame = new JFrame("Simulated Device Manager");
            frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
            frame.setResizable(false);

            final JPanel left = createListView();
            final JPanel right = createUpdateView();
            final JSplitPane splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT, true, left, right);
            splitPane.setEnabled(false);
            splitPane.setDividerSize(0);
            frame.getContentPane().add(splitPane, BorderLayout.CENTER);

            frame.setSize(800,400); //pixel size of frame in width then height
            frame.setVisible(true);

            if (twinClient != null) {
                loadDevices();
            }
        }

        private JPanel createUpdateView() {
            final JPanel panel = new JPanel();
            panel.setBorder(BorderFactory.createTitledBorder("Over the air updates"));

            final JButton sendButton = new JButton("Update");
            sendButton.addActionListener(new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    sendButton.setEnabled(false);

                    final UUID uuid = UUID.randomUUID();
                    final NewVersion newVersion = new NewVersion(uuid.toString(), String.format("https://example.com/%s.zip", uuid.toString()), "123ABC");

                    final Configuration config = new Configuration(uuid.toString());
                    config.setContent(new ConfigurationContent());
                    config.getContent().setDeviceContent(Collections.singletonMap("properties.desired.NewVersion", newVersion));
                    config.setTargetCondition("*");
                    config.setPriority(1);

                    try {
                        registryManager.addConfiguration(config);
                    } catch (IOException | IotHubException ex) {
                        logger.error(ex);
                    }

                    sendButton.setEnabled(true);
                }
            });
            panel.add(sendButton, BorderLayout.CENTER);
            return panel;
        }

        private JPanel createListView() {
            JPanel panel = new JPanel();
            panel.setBorder(BorderFactory.createTitledBorder("Available devices"));

            deviceListSource = new DefaultListModel();
            JList deviceList = new JList(deviceListSource);
            JScrollPane scrollPane = new JScrollPane(deviceList);
            panel.add(scrollPane, BorderLayout.NORTH);

            JPanel inputPanel = new JPanel();
            inputPanel.setBorder(BorderFactory.createTitledBorder("Actions"));
            JTextField intervalInput = new JTextField(3);
            intervalInput.setText("10");
            inputPanel.add(intervalInput, BorderLayout.NORTH);

            JButton sendButton = new JButton("Send");
            sendButton.addActionListener(new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    sendButton.setEnabled(false);

                    final int interval = Integer.valueOf(intervalInput.getText());
                    final Set<Pair> properties = new HashSet<>();
                    properties.add(new Pair("WeatherDataPatch", new WeatherDataPatch(interval)));
                    for(int index : deviceList.getSelectedIndices()) {
                        String deviceId = (String) deviceListSource.get(index);
                        DeviceTwinDevice twinDevice = new DeviceTwinDevice(deviceId);
                        twinDevice.setDesiredProperties(properties);
                        try {
                            twinClient.updateTwin(twinDevice);
                        } catch (IotHubException | IOException ex) {
                            logger.error(ex);
                        }
                    }
                    sendButton.setEnabled(true);
                }
            });
            inputPanel.add(sendButton, BorderLayout.SOUTH);
            panel.add(inputPanel, BorderLayout.SOUTH);
            return panel;
        }

        private void loadDevices() {
            try {
                Query qry = twinClient.queryTwin("select * from devices");
                while (twinClient.hasNextDeviceTwin(qry)) {
                    DeviceTwinDevice deviceTwin = twinClient.getNextDeviceTwin(qry);
                    deviceListSource.add(0, deviceTwin.getDeviceId());
                }
            } catch (IOException | IotHubException e) {
                logger.error(e);
            }
        }
    }


}
