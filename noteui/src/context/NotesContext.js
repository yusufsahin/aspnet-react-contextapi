import { createContext, useContext, useReducer, useEffect } from 'react';

// Create the context
const NotesContext = createContext();

// Define the initial state
const initialState = {
  notes: [],
  loading: false,
  error: null,
};

// Define the reducer to handle actions
const notesReducer = (state, action) => {
  switch (action.type) {
    case 'FETCH_NOTES_REQUEST':
      return { ...state, loading: true, error: null };
    case 'FETCH_NOTES_SUCCESS':
      return { ...state, loading: false, notes: action.payload };
    case 'FETCH_NOTES_FAILURE':
      return { ...state, loading: false, error: action.error };
    case 'ADD_NOTE':
      return { ...state, notes: [...state.notes, action.payload] };
    case 'UPDATE_NOTE':
      return {
        ...state,
        notes: state.notes.map((note) =>
          note.id === action.payload.id ? action.payload : note
        ),
      };
    case 'DELETE_NOTE':
      return {
        ...state,
        notes: state.notes.filter((note) => note.id !== action.payload),
      };
    default:
      return state;
  }
};

// Custom hook to use the Notes context
export const useNotes = () => useContext(NotesContext);

// Define the provider component
export const NotesProvider = ({ children }) => {
  const [state, dispatch] = useReducer(notesReducer, initialState);

  // Function to fetch notes from the API
  const fetchNotes = async () => {
    dispatch({ type: 'FETCH_NOTES_REQUEST' });
    try {
      const response = await fetch('http://localhost:5153/api/notes');
      const data = await response.json();
      dispatch({ type: 'FETCH_NOTES_SUCCESS', payload: data });
    } catch (error) {
      dispatch({ type: 'FETCH_NOTES_FAILURE', error: 'Failed to fetch notes' });
    }
  };

  // Function to add a note (API call)
  const addNote = async (note) => {
    const response = await fetch('http://localhost:5153/api/notes', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(note),
    });
    const newNote = await response.json();
    dispatch({ type: 'ADD_NOTE', payload: newNote });
  };

  // Function to update a note (API call)
  const updateNote = async (note) => {
    try {
      const response = await fetch(`http://localhost:5153/api/notes/${note.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(note),
      });
      
      if (response.ok) {
        // If the response is 204 (No Content), don't parse it as JSON
        const updatedNote = response.status === 204 ? note : await response.json();
        dispatch({ type: 'UPDATE_NOTE', payload: updatedNote });
      } else {
        throw new Error('Failed to update note');
      }
    } catch (error) {
      console.error('Error updating note:', error);
    }
  };
  
  const deleteNote = async (id) => {
    try {
      const response = await fetch(`http://localhost:5153/api/notes/${id}`, {
        method: 'DELETE',
      });
      
      if (response.ok) {
        // No need to parse the response as JSON if it's 204 No Content
        dispatch({ type: 'DELETE_NOTE', payload: id });
      } else {
        throw new Error('Failed to delete note');
      }
    } catch (error) {
      console.error('Error deleting note:', error);
    }
  };
  

  // Fetch notes on component mount
  useEffect(() => {
    fetchNotes();
  }, []);

  // Provide the state and actions to components
  return (
    <NotesContext.Provider value={{ ...state, addNote, updateNote, deleteNote }}>
      {children}
    </NotesContext.Provider>
  );
};
