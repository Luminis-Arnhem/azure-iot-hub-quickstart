package device.weather.azure;

public class HubClientException extends Exception {

    public HubClientException() {
        super();
    }

    public HubClientException(String message) {
        super(message);
    }

    public HubClientException(String message, Throwable cause) {
        super(message, cause);
    }
}
