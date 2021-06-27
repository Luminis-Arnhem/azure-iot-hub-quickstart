package manager.properties;

import java.io.IOException;
import java.util.Properties;

public class PropertyFileReader {

    private static final String appConfigFile = "/application.properties";
    private final Properties appProps = new Properties();

    public PropertyFileReader() throws IOException {
        appProps.load(getClass().getResourceAsStream(appConfigFile));
    }

    public String getProperty(String key) {
        return getProperty(key, null);
    }

    public String getProperty(String key, String defaultValue) {
        return appProps.getProperty(key, defaultValue);
    }
}
