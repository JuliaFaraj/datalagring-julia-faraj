import { useEffect, useState } from "react";

type Course = {
  id: number;
  courseCode: string;
  title: string;
  description: string;
};

function App() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [courseCode, setCourseCode] = useState("");
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  const fetchCourses = async () => {
    const response = await fetch("https://localhost:7032/courses");
    const data = await response.json();
    setCourses(data);
  };

  useEffect(() => {
    fetchCourses();
  }, []);

  const handleCreate = async () => {
    await fetch("https://localhost:7032/courses", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        courseCode,
        title,
        description,
      }),
    });

    setCourseCode("");
    setTitle("");
    setDescription("");
    fetchCourses();
  };

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Courses</h1>

      <h3>Add course</h3>
      <input
        placeholder="Course code"
        value={courseCode}
        onChange={(e) => setCourseCode(e.target.value)}
      />
      <input
        placeholder="Title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />
      <input
        placeholder="Description"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />
      <button onClick={handleCreate}>Create</button>

      <hr />

      {courses.length === 0 ? (
        <p>No courses found.</p>
      ) : (
        courses.map((c) => (
          <div key={c.id} style={{ marginBottom: "1rem" }}>
            <h3>{c.title}</h3>
            <p>{c.description}</p>
            <small>{c.courseCode}</small>
          </div>
        ))
      )}
    </div>
  );
}

export default App;
