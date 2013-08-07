using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public interface IInput
    {
        InputState GetState();
        void Update();
        IInput Map(String id, Object button);
        void Unmap(String id);
    }
}
