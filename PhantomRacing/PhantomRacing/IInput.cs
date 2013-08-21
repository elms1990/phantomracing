using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public interface IInput
    {
        /// <summary>
        /// Retrieves the state of this input.
        /// </summary>
        /// <returns>Current state.</returns>
        InputState GetState();

        /// <summary>
        /// Updates the state of all mapped buttons.
        /// </summary>
        void Update();


        /// <summary>
        /// Adds a new button to the mapping.
        /// </summary>
        /// <param name="id">Id of the virtual button.</param>
        /// <param name="button">Physical button.</param>
        /// <returns>An instance of this class.</returns>
        IInput Map(String id, Object button);

        /// <summary>
        /// Removes a button from mapping.
        /// </summary>
        /// <param name="id">Virtual button id.</param>
        void Unmap(String id);
    }
}
