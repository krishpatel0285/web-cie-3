document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('img').forEach(img => {
    img.addEventListener('error', () => {
      if (img.dataset.fallbackApplied) return;
      img.dataset.fallbackApplied = 'true';
      img.src = 'https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?auto=format&fit=crop&w=1200&q=80';
    });
  });
  const dateInput = document.querySelector('input[name="PreferredDate"]');
  if (dateInput) dateInput.min = new Date().toISOString().slice(0, 10);
});
