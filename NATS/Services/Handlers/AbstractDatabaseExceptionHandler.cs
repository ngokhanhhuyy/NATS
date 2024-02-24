namespace NATS.Services.Handlers;

public abstract class AbstractDatabaseExceptionHandler<TException> {
    public abstract void Handle(TException exception);
    
    public abstract string Schema { get; }

    public bool IsUniqueConstraintViolated { get; protected set; }

    public bool IsNotNullConstraintViolated { get; protected set; }
    
    public bool IsMaxLengthExceeded { get; protected set; }
    
    public bool IsForeignKeyConstraintViolated { get; protected set; }
    
    public string ViolatedTableName { get; protected set; }
    
    public string ViolatedFieldName { get; protected set; }
    
    public string ViolatedConstraintName { get; protected set; }
    
    public object ViolatedValue { get; protected set; }
}