using System;
using System.Text.Json;
using Confluent.Kafka;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "local-monitor-group",
    AutoOffsetReset = AutoOffsetReset.Latest // קורא הודעות בזמן אמת מהרגע שהוא נדלק
};

using var consumer = new ConsumerBuilder<string, string>(config).Build();
consumer.Subscribe("temperature-readings");

Console.WriteLine("🛡️ הצרכן המקומי רץ ומחכה להודעות מקפקא ב-Docker...");

while (true)
{
    try
    {
        var result = consumer.Consume(CancellationToken.None);
        var json = JsonDocument.Parse(result.Message.Value).RootElement;

        double temp = json.GetProperty("temperature").GetDouble();
        string sensor = json.GetProperty("sensor_id").GetString() ?? "Unknown";

        Console.WriteLine($"📊 [קפקא] התקבל דיווח מחיישן {sensor}: {temp}°C");

        if (temp > 35.0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"🚨 🔥 סכנה! טמפרטורה גבוהה מדי ב-{sensor}: {temp}°C! 🔥 🚨");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"שגיאה בקריאה מקפקא: {ex.Message}");
    }
}
