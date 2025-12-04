import axios from "axios";

// יצירת axios instance עם Base URL
const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL || "http://localhost:5091",
  // בפרודקשן זה יהיה: https://todolistnewsserver.onrender.com
});

// Interceptor לתפיסת שגיאות
api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error("API ERROR:", error);
    return Promise.reject(error);
  }
);

// שליפת כל המשימות
export const getTasks = async () => {
  const res = await api.get("/api/todoitems");
  return res.data;
};

// הוספת משימה חדשה
export const addTask = async (name) => {
  const res = await api.post("/api/todoitems", {
    name: name,
    isComplete: false,
  });
  return res.data;
};

// עדכון משימה (סימון V)
export const setCompleted = async (id, isComplete) => {
  const res = await api.put(`/api/todoitems/${id}`, {
    isComplete: isComplete,
    // שים לב: אפשר לשלוח רק את השדה שמשתנה – השרת ימלא את השאר
  });
  return res.data;
};

// מחיקת משימה
export const deleteTask = async (id) => {
  await api.delete(`/api/todoitems/${id}`);
};