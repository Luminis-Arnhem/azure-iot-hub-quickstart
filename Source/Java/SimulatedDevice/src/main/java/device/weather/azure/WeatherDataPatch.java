package device.weather.azure;

public class WeatherDataPatch {

    public int ReportingRateSeconds;

    @Override
    public String toString() {
        return "WeatherDataPatch{" +
                "ReportingRateSeconds=" + ReportingRateSeconds +
                '}';
    }
}
