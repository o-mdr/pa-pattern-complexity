namespace PatternPA.Core.Model
{
    /// <summary>
    /// Abstract class that defines domain object 
    /// that can be identified by its identity
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Assignment is fully managed by ORM
        /// </summary>
        public virtual long Id { get; private set; }
    }
}