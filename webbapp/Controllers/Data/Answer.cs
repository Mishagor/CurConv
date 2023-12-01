namespace webbapp.Controllers.Data
{
    /// <summary>
    /// Container for reponse data.
    /// </summary>
    /// <typeparam name="T">The type of the value returned in this answer.</typeparam>
    public class Answer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Answer{T}"/> class, using classifier parameters.
        /// </summary>
        /// <param name="ok">The <see cref="bool"/> specifying response success.</param>
        /// <param name="value">The response's value object.</param>
        public Answer(bool ok, T value)
        {
            this.Success = ok;
            this.Response = value;
        }

        /// <summary>
        /// Gets a value indicating whether the request is successfull.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets a reponse's value.
        /// </summary>
        public T Response { get; private set; }
    }
}
