using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Text.Json;

namespace KafkaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemperatureController : ControllerBase
    {
        [HttpPost("report")]
        public async Task<IActionResult>Report([FromBody] TemperatureRecord record)
        {
            var config = new ProducerConfig{BootstrapServers ="localhost:9092"};
            using var producer = new ProducerBuilder<string ,string>(config).Build();

            var messageValue = JsonSerializer.Serialize(new
            {
                sensor_id = record.SensorId,
                temperature = record.Temperature,
               timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()

            });
            await producer.ProduceAsync("temperature-readings", new Message<string, string> { Value = messageValue });
            return Ok(new { status = "Success", message = "הנתון נדחף לקפקא!" });
        }
    }
    public class TemperatureRecord
{
    public string SensorId { get; set; } = string.Empty;
    public double Temperature { get; set; }
}


}