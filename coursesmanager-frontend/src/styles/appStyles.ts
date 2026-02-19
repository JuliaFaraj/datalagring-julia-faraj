import type { CSSProperties } from "react";

export const styles: Record<string, CSSProperties> = {
  page: {
    minHeight: "100vh",
    position: "relative",
    overflow: "hidden",
    background:
      "linear-gradient(180deg, #fff7fb 0%, #ffeaf4 50%, #fff7fb 100%)",
    color: "#3a2a2f",
    fontFamily:
      "system-ui, -apple-system, Segoe UI, Roboto, Arial, sans-serif",
  },

  bgGlowTop: {
    position: "absolute",
    inset: "-250px auto auto -250px",
    width: 520,
    height: 520,
    filter: "blur(80px)",
    background:
      "radial-gradient(circle, rgba(255,182,193,0.45), transparent 65%)",
    pointerEvents: "none",
  },

  bgGlowBottom: {
    position: "absolute",
    inset: "auto -250px -250px auto",
    width: 520,
    height: 520,
    filter: "blur(90px)",
    background:
      "radial-gradient(circle, rgba(255,192,203,0.35), transparent 65%)",
    pointerEvents: "none",
  },

  container: {
    width: "min(1400px, calc(100% - 40px))",
    margin: "0 auto",
    padding: "32px 0 50px",
  },

  header: {
    display: "flex",
    alignItems: "flex-start",
    justifyContent: "space-between",
    gap: 16,
    marginBottom: 24,
  },

  kicker: {
    fontSize: 12,
    letterSpacing: 1.5,
    textTransform: "uppercase",
    opacity: 0.6,
  },

  h1: {
    margin: "6px 0 6px",
    fontSize: 40,
    lineHeight: 1.05,
    letterSpacing: -0.5,
  },

  sub: {
    margin: 0,
    opacity: 0.7,
    maxWidth: 520,
  },

  headerRight: {
    display: "flex",
    gap: 12,
    flexWrap: "wrap",
  },

  statCard: {
    borderRadius: 16,
    padding: "12px 14px",
    border: "1px solid rgba(255,182,193,0.4)",
    background: "rgba(255,255,255,0.9)",
    minWidth: 150,
  },

  statLabel: {
    fontSize: 12,
    opacity: 0.6,
  },

  statValue: {
    marginTop: 4,
    fontSize: 20,
    fontWeight: 700,
  },

  toolbar: {
    display: "flex",
    gap: 12,
    alignItems: "center",
    justifyContent: "space-between",
    marginBottom: 20,
  },

  searchWrap: {
    position: "relative",
    flex: 1,
    maxWidth: 620,
  },

  searchIcon: {
    position: "absolute",
    left: 12,
    top: 10,
    opacity: 0.5,
    fontSize: 14,
  },

  searchInput: {
    width: "100%",
    padding: "10px 12px 10px 34px",
    borderRadius: 14,
    border: "1px solid rgba(255,182,193,0.4)",
    outline: "none",
    background: "white",
    color: "#3a2a2f",
  },

  alertError: {
    borderRadius: 16,
    border: "1px solid rgba(239,68,68,0.3)",
    background: "rgba(239,68,68,0.08)",
    padding: "12px 14px",
    marginBottom: 16,
  },

  alertTitle: {
    fontWeight: 700,
    marginBottom: 4,
  },

  alertText: {
    opacity: 0.8,
  },

  centerText: {
    padding: "24px 8px",
    opacity: 0.7,
  },

  grid: {
    display: "grid",
    gridTemplateColumns: "repeat(auto-fill, minmax(280px, 1fr))",
    gap: 16,
  },

  card: {
    borderRadius: 18,
    border: "1px solid rgba(255,182,193,0.35)",
    background: "white",
    boxShadow: "0 8px 25px rgba(255,182,193,0.25)",
    overflow: "hidden",
    display: "flex",
    flexDirection: "column",
    minHeight: 220,
    transition: "all 0.2s ease",

  },

  cardTop: {
    padding: 16,
    flex: 1,
  },

  badgeRow: {
    display: "flex",
    gap: 8,
    alignItems: "center",
    flexWrap: "wrap",
    marginBottom: 10,
  },

  badge: {
    fontSize: 12,
    fontWeight: 700,
    padding: "6px 10px",
    borderRadius: 999,
    border: "1px solid rgba(255,105,180,0.3)",
    background: "rgba(255,192,203,0.2)",
    color: "#b83280",
  },

  badgeWarn: {
    fontSize: 12,
    fontWeight: 700,
    padding: "6px 10px",
    borderRadius: 999,
    border: "1px solid rgba(239,68,68,0.3)",
    background: "rgba(239,68,68,0.1)",
    color: "#b91c1c",
  },

  cardTitle: {
    margin: "0 0 8px",
    fontSize: 18,
  },

  cardDesc: {
    margin: 0,
    opacity: 0.75,
    lineHeight: 1.45,
  },

  cardBottom: {
    padding: 14,
    borderTop: "1px solid rgba(255,182,193,0.25)",
    background: "rgba(255,240,245,0.7)",
  },

  editor: {
    display: "grid",
    gap: 10,
  },

  field: {
    display: "grid",
    gap: 6,
  },

  label: {
    fontSize: 12,
    opacity: 0.7,
  },

  input: {
    padding: "10px 12px",
    borderRadius: 14,
    border: "1px solid rgba(255,182,193,0.4)",
    outline: "none",
    background: "white",
    color: "#3a2a2f",
  },

  textarea: {
    padding: "10px 12px",
    borderRadius: 14,
    border: "1px solid rgba(255,182,193,0.4)",
    outline: "none",
    background: "white",
    color: "#3a2a2f",
    resize: "vertical",
  },

  actions: {
    display: "flex",
    gap: 10,
    alignItems: "center",
    marginTop: 6,
  },

  button: {
    padding: "10px 14px",
    borderRadius: 14,
    border: "1px solid rgba(255,182,193,0.4)",
    background: "white",
    color: "#3a2a2f",
    cursor: "pointer",
    fontWeight: 600,
  },

  buttonPrimary: {
    border: "1px solid rgba(255,105,180,0.5)",
    background: "rgba(255,192,203,0.35)",
    color: "#7a1f4d",
  },

  buttonGhost: {
    background: "rgba(255,240,245,0.8)",
  },

  buttonDisabled: {
    opacity: 0.5,
    cursor: "not-allowed",
  },

  inlineError: {
    display: "flex",
    gap: 8,
    alignItems: "center",
    padding: "8px 10px",
    borderRadius: 14,
    border: "1px solid rgba(239,68,68,0.3)",
    background: "rgba(239,68,68,0.1)",
    fontSize: 13,
  },

  inlineErrorDot: {
    width: 8,
    height: 8,
    borderRadius: 999,
    background: "rgba(239,68,68,0.9)",
  },

  tinyHint: {
    marginTop: 4,
    fontSize: 12,
    opacity: 0.6,
  },

  footer: {
    marginTop: 24,
    paddingTop: 14,
    borderTop: "1px solid rgba(255,182,193,0.3)",
    fontSize: 12,
  },
};
