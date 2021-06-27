package manager.azure;

public class WeatherDataPatch {

    public int ReportingRateSeconds;

    public WeatherDataPatch() {
        // nothing
    }

    public WeatherDataPatch(int reportingRateSeconds) {
        ReportingRateSeconds = reportingRateSeconds;
    }

    @Override
    public String toString() {
        return "WeatherDataPatch{" +
                "ReportingRateSeconds=" + ReportingRateSeconds +
                '}';
    }
}
