(function () {
  "use strict";

  var prefersReducedMotion = window.matchMedia
    ? window.matchMedia("(prefers-reduced-motion: reduce)")
    : { matches: false };

  function initNavbar() {
    var navbar = document.getElementById("siteNavbar");
    if (!navbar) {
      return;
    }

    var updateNavbar = function () {
      navbar.classList.toggle("is-scrolled", window.scrollY > 20);
    };

    updateNavbar();
    window.addEventListener("scroll", updateNavbar, { passive: true });
  }

  function initNavigation() {
    var navLinks = Array.prototype.slice.call(
      document.querySelectorAll("[data-nav-link]")
    );
    if (navLinks.length === 0) {
      return;
    }

    var navbarCollapse = document.getElementById("mainNavigation");
    var sections = navLinks
      .map(function (link) {
        var href = link.getAttribute("href") || "";
        if (href.charAt(0) !== "#") {
          return null;
        }
        var section = document.getElementById(href.slice(1));
        return section ? { link: link, section: section } : null;
      })
      .filter(Boolean);

    var setActiveLink = function (activeLink) {
      navLinks.forEach(function (link) {
        var isActive = link === activeLink;
        link.classList.toggle("active", isActive);
        if (isActive) {
          link.setAttribute("aria-current", "true");
        } else {
          link.removeAttribute("aria-current");
        }
      });
    };

    var updateActiveLink = function () {
      var scrollPosition = window.scrollY + window.innerHeight * 0.35;
      var current = sections[0];

      sections.forEach(function (entry) {
        if (entry.section.offsetTop <= scrollPosition) {
          current = entry;
        }
      });

      if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 2) {
        current = sections[sections.length - 1];
      }

      if (current) {
        setActiveLink(current.link);
      }
    };

    updateActiveLink();
    window.addEventListener("scroll", updateActiveLink, { passive: true });
    window.addEventListener("resize", updateActiveLink);

    navLinks.forEach(function (link) {
      link.addEventListener("click", function () {
        if (
          navbarCollapse &&
          navbarCollapse.classList.contains("show") &&
          window.bootstrap
        ) {
          var collapse = window.bootstrap.Collapse.getOrCreateInstance(
            navbarCollapse,
            { toggle: false }
          );
          collapse.hide();
        }
      });
    });
  }

  function initBackToTop() {
    var button = document.getElementById("backToTop");
    if (!button) {
      return;
    }

    var updateVisibility = function () {
      button.classList.toggle("is-visible", window.scrollY > 400);
    };

    updateVisibility();
    window.addEventListener("scroll", updateVisibility, { passive: true });

    button.addEventListener("click", function () {
      window.scrollTo({
        top: 0,
        behavior: prefersReducedMotion.matches ? "auto" : "smooth"
      });
    });
  }

  function initReveal() {
    var elements = Array.prototype.slice.call(
      document.querySelectorAll("[data-reveal]")
    );
    if (elements.length === 0) {
      return;
    }

    if (prefersReducedMotion.matches || !("IntersectionObserver" in window)) {
      elements.forEach(function (element) {
        element.classList.add("reveal", "is-visible");
      });
      return;
    }

    elements.forEach(function (element) {
      element.classList.add("reveal");
    });

    var observer = new IntersectionObserver(
      function (entries, currentObserver) {
        entries.forEach(function (entry) {
          if (entry.isIntersecting) {
            entry.target.classList.add("is-visible");
            currentObserver.unobserve(entry.target);
          }
        });
      },
      { threshold: 0.15, rootMargin: "0px 0px -40px 0px" }
    );

    elements.forEach(function (element) {
      observer.observe(element);
    });
  }

  function initPhotoFallback() {
    var photo = document.querySelector("[data-profile-photo]");
    if (!photo) {
      return;
    }

    var showFallback = function () {
      photo.style.display = "none";
    };

    photo.addEventListener("error", showFallback);

    if (photo.complete && photo.naturalWidth === 0) {
      showFallback();
    }
  }

  function init() {
    initNavbar();
    initNavigation();
    initBackToTop();
    initReveal();
    initPhotoFallback();
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", init);
  } else {
    init();
  }
})();
