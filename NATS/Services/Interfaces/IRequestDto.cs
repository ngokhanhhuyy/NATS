namespace NATS.Services.Interfaces;

public interface IRequestDto<out TRequestDto> {
    TRequestDto TransformValues();
}